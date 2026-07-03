using System;
using Moq;
using Xunit;

// Assumption: production namespace matches the file path.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderCustomCustomerLoginParameterizationTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesBoundEmailParameter_NotConcatenatedIntoSql()
        {
            // Arrange
            var configFile = new Mock<ConfigFile>(MockBehavior.Loose);
            configFile.Setup(c => c.Get(It.IsAny<string>())).Returns("test.db");
            var provider = new SqliteDbProvider(configFile.Object);

            // Act
            var method = typeof(SqliteDbProvider).GetMethod("CustomCustomerLogin");
            Assert.NotNull(method);
            var literals = MethodStringLiteralInspector.GetAllStringLiterals(method!);

            // Assert
            Assert.Contains("select * from CustomerLogin where email = @email", literals);
            Assert.DoesNotContain("select * from CustomerLogin where email = '\" + email", string.Join("\n", literals));
        }

        private static class MethodStringLiteralInspector
        {
            public static string[] GetAllStringLiterals(System.Reflection.MethodInfo method)
            {
                var module = method.Module;
                var body = method.GetMethodBody();
                Assert.NotNull(body);
                var il = body!.GetILAsByteArray();
                Assert.NotNull(il);

                var literals = new System.Collections.Generic.List<string>();
                int i = 0;
                while (i < il!.Length)
                {
                    byte op = il[i++];
                    if (op == 0xFE)
                    {
                        if (i >= il.Length) break;
                        op = (byte)(0xFE00 | il[i++]);
                    }

                    if (op == 0x72)
                    {
                        if (i + 4 > il.Length) break;
                        int token = BitConverter.ToInt32(il, i);
                        i += 4;
                        try { literals.Add(module.ResolveString(token)); } catch { }
                        continue;
                    }

                    if (op == 0x20 || op == 0x28 || op == 0x6F || op == 0x73 || op == 0x74 || op == 0x7A || op == 0x7B || op == 0x7C || op == 0x7D || op == 0x7E || op == 0x7F)
                        i += 4;
                    else if (op == 0x21)
                        i += 8;
                    else if (op == 0x22)
                        i += 4;
                    else if (op == 0x23)
                        i += 8;
                    else if (op == 0x0E || op == 0x10 || op == 0x11 || op == 0x13 || op == 0x1F)
                        i += 1;
                    else if (op >= 0x2B && op <= 0x37)
                        i += 1;
                    else if (op >= 0x38 && op <= 0x44)
                        i += 4;
                }

                return literals.ToArray();
            }
        }
    }
}
