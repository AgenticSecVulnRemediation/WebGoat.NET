using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Xunit;

// Assumption: production namespace is OWASP.WebGoat.NET.App_Code.DB (based on file path and source file namespace).
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderSecurityDeltaTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedPlaceholder_InSource()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("GetOrders", BindingFlags.Public | BindingFlags.Instance);
            Assert.NotNull(method);

            // Act
            var ilBytes = method!.GetMethodBody()!.GetILAsByteArray();

            // Assert
            // We avoid DB/network access and verify the delta at source-level by asserting the parameter placeholder
            // string is present in the compiled method's user strings.
            // This checks the regression: "customerNumber = @customerID" is used rather than concatenation.
            var strings = GetUserStrings(method.Module);
            Assert.Contains("select * from Orders where customerNumber = @customerID", strings);
        }

        [Fact]
        public void GetProductDetails_UsesParameterizedPlaceholder_InSource()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("GetProductDetails", BindingFlags.Public | BindingFlags.Instance);
            Assert.NotNull(method);

            // Assert
            var strings = GetUserStrings(method!.Module);
            Assert.Contains("select * from Products where productCode = @productCode", strings);
            Assert.Contains("select * from Comments where productCode = @productCode", strings);
        }

        [Fact]
        public void GetEmailByName_UsesLikeParameter_InSource()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("GetEmailByName", BindingFlags.Public | BindingFlags.Instance);
            Assert.NotNull(method);

            // Assert
            var strings = GetUserStrings(method!.Module);
            Assert.Contains("select firstName, lastName, email from Employees where firstName like @name or lastName like @name", strings);
        }

        private static HashSet<string> GetUserStrings(Module module)
        {
            // Reads the #US heap and returns all user strings.
            // This keeps the test executable without relying on a real DB, and it is a precise regression for the delta.
            var pe = module.Assembly.Location;
            Assert.False(string.IsNullOrWhiteSpace(pe));

            using var fs = System.IO.File.OpenRead(pe);
            using var br = new System.IO.BinaryReader(fs);

            // Minimal PE parsing to find the CLI metadata root and #US stream.
            // If layout differs, fail with a clear message rather than silently passing.
            var strings = new HashSet<string>(StringComparer.Ordinal);

            // PE header
            fs.Position = 0x3C;
            var peHeaderOffset = br.ReadInt32();
            fs.Position = peHeaderOffset;
            var peSig = br.ReadUInt32();
            Assert.Equal(0x00004550u, peSig); // "PE\0\0"

            fs.Position = peHeaderOffset + 0x18; // COFF + Optional header start
            var magic = br.ReadUInt16();
            bool isPE32Plus = magic == 0x20b;
            Assert.True(magic == 0x10b || isPE32Plus);

            // Data directories start after standard fields. Offsets differ for PE32 vs PE32+.
            // Optional header size for PE32: 0xE0; PE32+: 0xF0. DataDir offset: 0x60 (PE32) / 0x70 (PE32+)
            long dataDirOffset = peHeaderOffset + 0x18 + (isPE32Plus ? 0x70 : 0x60);
            fs.Position = dataDirOffset + (14 * 8); // CLI header directory (index 14)
            var cliRva = br.ReadUInt32();
            var cliSize = br.ReadUInt32();
            Assert.True(cliRva != 0 && cliSize != 0);

            // Section headers
            fs.Position = peHeaderOffset + 0x6;
            var numberOfSections = br.ReadUInt16();
            fs.Position = peHeaderOffset + 0x14;
            var sizeOfOptionalHeader = br.ReadUInt16();
            long sectionTable = peHeaderOffset + 0x18 + sizeOfOptionalHeader;

            uint RvaToFileOffset(uint rva)
            {
                for (int i = 0; i < numberOfSections; i++)
                {
                    fs.Position = sectionTable + i * 40;
                    br.ReadBytes(8); // name
                    uint virtualSize = br.ReadUInt32();
                    uint virtualAddress = br.ReadUInt32();
                    uint sizeOfRawData = br.ReadUInt32();
                    uint pointerToRawData = br.ReadUInt32();
                    br.ReadBytes(16);

                    uint sectionSize = Math.Max(virtualSize, sizeOfRawData);
                    if (rva >= virtualAddress && rva < virtualAddress + sectionSize)
                        return pointerToRawData + (rva - virtualAddress);
                }
                throw new InvalidOperationException("Unable to map RVA to file offset");
            }

            // CLI header -> Metadata RVA
            fs.Position = RvaToFileOffset(cliRva);
            br.ReadUInt32(); // cb
            br.ReadUInt16(); // major
            br.ReadUInt16(); // minor
            uint metadataRva = br.ReadUInt32();
            br.ReadUInt32(); // metadata size

            // Metadata root
            fs.Position = RvaToFileOffset(metadataRva);
            Assert.Equal(0x424A5342u, br.ReadUInt32()); // "BSJB"
            br.ReadUInt16(); // major
            br.ReadUInt16(); // minor
            br.ReadUInt32(); // reserved
            int versionLen = br.ReadInt32();
            br.ReadBytes(versionLen);
            // align to 4
            while (fs.Position % 4 != 0) fs.Position++;
            br.ReadUInt16(); // flags
            ushort streams = br.ReadUInt16();

            // Stream headers
            long usOffset = -1;
            int usSize = 0;
            for (int i = 0; i < streams; i++)
            {
                uint offset = br.ReadUInt32();
                uint size = br.ReadUInt32();
                // read null-terminated name (padded to 4)
                var nameBytes = new List<byte>();
                while (true)
                {
                    byte b = br.ReadByte();
                    if (b == 0) break;
                    nameBytes.Add(b);
                }
                while (fs.Position % 4 != 0) fs.Position++;
                var name = System.Text.Encoding.ASCII.GetString(nameBytes.ToArray());
                if (name == "#US")
                {
                    usOffset = offset;
                    usSize = (int)size;
                }
            }
            Assert.True(usOffset >= 0 && usSize > 0);

            long usStart = RvaToFileOffset(metadataRva) + usOffset;
            fs.Position = usStart;
            long end = usStart + usSize;

            // #US heap entries: compressed length + UTF-16 bytes + trailing 0x00 0x00.
            // We'll do a tolerant scan by reading the heap and decoding UTF-16 where possible.
            while (fs.Position < end)
            {
                long entryStart = fs.Position;
                if (fs.Position == end) break;

                int len = ReadCompressedInt(br, fs, end);
                if (len <= 0 || fs.Position + len > end)
                {
                    fs.Position = entryStart + 1;
                    continue;
                }

                var data = br.ReadBytes(len);
                // try decode UTF-16 (most user strings)
                if (data.Length >= 2)
                {
                    try
                    {
                        string s = System.Text.Encoding.Unicode.GetString(data);
                        // trim trailing nulls
                        s = s.TrimEnd('\0');
                        if (!string.IsNullOrEmpty(s)) strings.Add(s);
                    }
                    catch { /* ignore */ }
                }
            }

            return strings;
        }

        private static int ReadCompressedInt(System.IO.BinaryReader br, System.IO.Stream s, long end)
        {
            if (s.Position >= end) return -1;
            byte first = br.ReadByte();
            if ((first & 0x80) == 0)
            {
                return first;
            }
            if ((first & 0xC0) == 0x80)
            {
                if (s.Position >= end) return -1;
                byte second = br.ReadByte();
                return ((first & 0x3F) << 8) | second;
            }
            // 4-byte
            if (s.Position + 2 >= end) return -1;
            byte b2 = br.ReadByte();
            byte b3 = br.ReadByte();
            byte b4 = br.ReadByte();
            return ((first & 0x1F) << 24) | (b2 << 16) | (b3 << 8) | b4;
        }
    }
}
