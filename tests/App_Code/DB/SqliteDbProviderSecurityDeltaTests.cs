using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderSecurityDeltaTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsert_InSource()
        {
            var strings = GetUserStrings(typeof(SqliteDbProvider).Module);
            Assert.Contains("insert into Comments(productCode, email, comment) values (@productCode, @Email, @Comment);", strings);
        }

        [Fact]
        public void GetProductDetails_UsesParameterizedQueries_InSource()
        {
            var strings = GetUserStrings(typeof(SqliteDbProvider).Module);
            Assert.Contains("select * from Products where productCode = @productCode", strings);
            Assert.Contains("select * from Comments where productCode = @productCode", strings);
        }

        private static HashSet<string> GetUserStrings(Module module)
        {
            // Same implementation as in MySqlDbProviderSecurityDeltaTests; duplicated to keep each test file standalone.
            var pe = module.Assembly.Location;
            Assert.False(string.IsNullOrWhiteSpace(pe));

            using var fs = System.IO.File.OpenRead(pe);
            using var br = new System.IO.BinaryReader(fs);

            var strings = new HashSet<string>(StringComparer.Ordinal);

            fs.Position = 0x3C;
            var peHeaderOffset = br.ReadInt32();
            fs.Position = peHeaderOffset;
            Assert.Equal(0x00004550u, br.ReadUInt32());

            fs.Position = peHeaderOffset + 0x18;
            var magic = br.ReadUInt16();
            bool isPE32Plus = magic == 0x20b;
            Assert.True(magic == 0x10b || isPE32Plus);

            long dataDirOffset = peHeaderOffset + 0x18 + (isPE32Plus ? 0x70 : 0x60);
            fs.Position = dataDirOffset + (14 * 8);
            var cliRva = br.ReadUInt32();
            var cliSize = br.ReadUInt32();
            Assert.True(cliRva != 0 && cliSize != 0);

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
                    br.ReadBytes(8);
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

            fs.Position = RvaToFileOffset(cliRva);
            br.ReadUInt32();
            br.ReadUInt16();
            br.ReadUInt16();
            uint metadataRva = br.ReadUInt32();
            br.ReadUInt32();

            fs.Position = RvaToFileOffset(metadataRva);
            Assert.Equal(0x424A5342u, br.ReadUInt32());
            br.ReadUInt16();
            br.ReadUInt16();
            br.ReadUInt32();
            int versionLen = br.ReadInt32();
            br.ReadBytes(versionLen);
            while (fs.Position % 4 != 0) fs.Position++;
            br.ReadUInt16();
            ushort streams = br.ReadUInt16();

            long usOffset = -1;
            int usSize = 0;
            for (int i = 0; i < streams; i++)
            {
                uint offset = br.ReadUInt32();
                uint size = br.ReadUInt32();
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

            while (fs.Position < end)
            {
                long entryStart = fs.Position;
                int len = ReadCompressedInt(br, fs, end);
                if (len <= 0 || fs.Position + len > end)
                {
                    fs.Position = entryStart + 1;
                    continue;
                }

                var data = br.ReadBytes(len);
                if (data.Length >= 2)
                {
                    try
                    {
                        string s = System.Text.Encoding.Unicode.GetString(data).TrimEnd('\0');
                        if (!string.IsNullOrEmpty(s)) strings.Add(s);
                    }
                    catch { }
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
            if (s.Position + 2 >= end) return -1;
            byte b2 = br.ReadByte();
            byte b3 = br.ReadByte();
            byte b4 = br.ReadByte();
            return ((first & 0x1F) << 24) | (b2 << 16) | (b3 << 8) | b4;
        }
    }
}
