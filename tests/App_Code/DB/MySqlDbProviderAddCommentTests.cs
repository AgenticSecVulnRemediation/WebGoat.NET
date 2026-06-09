using System;
using System.Collections.Generic;
using System.Reflection;
using Moq;
using Xunit;

// Assumptions:
// - Source namespace is OWASP.WebGoat.NET.App_Code.DB.
// - Delta test asserts AddComment now uses SQL parameters (@productCode, @Email, @Comment)
//   and does not inline comment/email/productCode into the SQL string.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderAddCommentTests
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
        public void AddComment_UsesParameterizedInsertAndDoesNotConcatenateInputs()
        {
            // Arrange
            var provider = new MySqlDbProvider(CreateConfigFileStub());

            // Act
            var method = typeof(MySqlDbProvider).GetMethod("AddComment");
            Assert.NotNull(method);

            var strings = GetUserStrings(method!);

            // Assert: new parameterized insert statement and parameter names exist
            Assert.Contains("insert into Comments(productCode, email, comment) values (@productCode, @Email, @Comment);", strings);
            Assert.Contains("@productCode", strings);
            Assert.Contains("@Email", strings);
            Assert.Contains("@Comment", strings);

            // Assert: old concatenation fragments should not be present
            Assert.DoesNotContain("values ('\"", strings);
            Assert.DoesNotContain("'\",\"'\"", strings);
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
                if (op == 0xFE) { i++; continue; }

                if (op == 0x72)
                {
                    int token = BitConverter.ToInt32(il, i);
                    i += 4;
                    try { strings.Add(module.ResolveString(token)); } catch { }
                    continue;
                }

                if (op is 0x28 or 0x6F) { i += 4; continue; }
                if (op is 0x2A or 0x00) continue;
                break;
            }

            return strings;
        }
    }
}
