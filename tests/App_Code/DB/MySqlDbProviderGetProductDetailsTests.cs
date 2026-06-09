using System;
using System.Collections.Generic;
using System.Reflection;
using Moq;
using Xunit;

// Assumptions:
// - Source namespace is OWASP.WebGoat.NET.App_Code.DB.
// - Delta test asserts the updated GetProductDetails query uses @productCode parameter and does not inline productCode.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsTests
    {
        private static ConfigFile CreateConfigFileStub()
        {
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns<string>(key =>
            {
                if (key == DbConstants.KEY_HOST) return "localhost";
                if (key == DbConstants.KEY_PORT) return "3306";
                if (key == DbConstants.KEY_DATABASE) return "webgoat";
                if (key == DbConstants.KEY_UID) return "user";
                if (key == DbConstants.KEY_PWD) return "pwd";
                if (key == DbConstants.KEY_CLIENT_EXEC) return "mysql";
                return string.Empty;
            });
            return config.Object;
        }

        [Fact]
        public void GetProductDetails_UsesParameterizedQueriesForProductsAndComments()
        {
            // Arrange
            var provider = new MySqlDbProvider(CreateConfigFileStub());

            // Act
            var method = typeof(MySqlDbProvider).GetMethod("GetProductDetails");
            Assert.NotNull(method);

            var strings = GetUserStrings(method!);

            // Assert: parameterized SQL introduced by fix
            Assert.Contains("select * from Products where productCode = @productCode", strings);
            Assert.Contains("select * from Comments where productCode = @productCode", strings);
            Assert.Contains("@productCode", strings);

            // Assert: old concatenated pattern should not exist
            Assert.DoesNotContain("select * from Products where productCode = '\"", strings);
            Assert.DoesNotContain("select * from Comments where productCode = '\"", strings);
        }

        private static IReadOnlyCollection<string> GetUserStrings(MethodInfo method)
        {
            var module = method.Module;
            var body = method.GetMethodBody();
            if (body == null) return Array.Empty<string>();

            var il = body.GetILAsByteArray();
            if (il == null) return Array.Empty<string>();

            var strings = new List<string>();
            int i = 0;
            while (i < il.Length)
            {
                byte op = il[i++];
                if (op == 0xFE)
                {
                    i++; // skip second byte
                    continue;
                }

                if (op == 0x72) // ldstr
                {
                    int token = BitConverter.ToInt32(il, i);
                    i += 4;
                    try { strings.Add(module.ResolveString(token)); } catch { }
                    continue;
                }

                if (op is 0x28 or 0x6F) { i += 4; continue; } // call/callvirt
                if (op is 0x2A or 0x00) continue; // ret/nop

                // unknown: stop
                break;
            }

            return strings;
        }
    }
}
