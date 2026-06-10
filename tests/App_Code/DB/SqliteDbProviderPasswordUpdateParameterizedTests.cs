using System;
using System.Reflection;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderPasswordUpdateParameterizedTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameters_InsteadOfStringConcatenation()
        {
            // Arrange
            // Fix changed the SQL template from concatenation to:
            // "UPDATE CustomerLogin SET password = @password WHERE customerNumber = @customerNumber"
            var method = typeof(SqliteDbProvider).GetMethod("UpdateCustomerPassword");
            Assert.NotNull(method);

            // Act
            var il = method!.GetMethodBody()!.GetILAsByteArray();
            var strings = IlStringExtractor.ExtractStrings(method.Module, il);

            // Assert
            Assert.Contains(strings, s => s.Contains("UPDATE CustomerLogin SET password = @password", StringComparison.Ordinal));
            Assert.Contains(strings, s => s.Contains("WHERE customerNumber = @customerNumber", StringComparison.Ordinal));

            // Old vulnerable pattern should not be present.
            Assert.DoesNotContain(strings, s => s.Contains("update CustomerLogin set password = '\"", StringComparison.OrdinalIgnoreCase));
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
                    if (op == 0x72)
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
                        case 0x28:
                        case 0x6F:
                        case 0x7B:
                        case 0x7C:
                        case 0x80:
                        case 0x8D:
                            i += 4;
                            break;
                        case 0x1F:
                        case 0x11:
                        case 0x13:
                        case 0x0E:
                        case 0x10:
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
