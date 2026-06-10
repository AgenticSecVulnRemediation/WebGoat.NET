using System;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderUserIdLookupTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UsesAtParameters_ForUserIdLookup()
        {
            // Arrange
            // Same behavioral change as PR #1411: use @UserName/@ApplicationId in the initial SELECT.
            var method = typeof(SQLiteProfileProvider).GetMethod(
                "GetPropertyValuesFromDatabase",
                BindingFlags.NonPublic | BindingFlags.Static);

            Assert.NotNull(method);

            // Act
            var il = method!.GetMethodBody()!.GetILAsByteArray();
            var module = method.Module;
            var strings = IlStringExtractor.ExtractStrings(module, il);

            // Assert
            Assert.Contains(strings, s => s.Contains("LoweredUsername = @UserName", StringComparison.Ordinal));
            Assert.Contains(strings, s => s.Contains("ApplicationId = @ApplicationId", StringComparison.Ordinal));

            Assert.DoesNotContain(strings, s => s.Contains("LoweredUsername = $UserName", StringComparison.Ordinal));
            Assert.DoesNotContain(strings, s => s.Contains("ApplicationId = $ApplicationId", StringComparison.Ordinal));
        }

        private static class IlStringExtractor
        {
            public static string[] ExtractStrings(Module module, byte[] il)
            {
                var list = new System.Collections.Generic.List<string>();

                int i = 0;
                while (i < il.Length)
                {
                    byte op = il[i++];

                    if (op == 0x72) // ldstr
                    {
                        int token = BitConverter.ToInt32(il, i);
                        i += 4;
                        list.Add(module.ResolveString(token));
                        continue;
                    }

                    if (op == 0xFE)
                    {
                        i++;
                        continue;
                    }

                    switch (op)
                    {
                        case 0x28: // call
                        case 0x6F: // callvirt
                        case 0x7B: // ldfld
                        case 0x7C: // ldflda
                        case 0x80: // stfld
                        case 0x8D: // newarr
                            i += 4;
                            break;
                        case 0x1F: // ldc.i4.s
                        case 0x11: // ldloc.s
                        case 0x13: // stloc.s
                        case 0x0E: // ldarg.s
                        case 0x10: // starg.s
                            i += 1;
                            break;
                        default:
                            break;
                    }
                }

                return list.ToArray();
            }
        }
    }
}
