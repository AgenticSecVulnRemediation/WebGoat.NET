using System;
using System.Data;
using Moq;
using Xunit;

// Assumption: production namespace matches the file path.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsParameterizationTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedProductCode_PreservesRelationName()
        {
            // Arrange
            var configFile = new Mock<ConfigFile>(MockBehavior.Loose);
            configFile.Setup(c => c.Get(It.IsAny<string>())).Returns("test.db");

            var provider = new SqliteDbProvider(configFile.Object);

            // The fixed code uses parameters in both Products and Comments queries.
            // We'll validate this via IL string literal inspection (delta test).

            // Act
            var literals = MethodStringLiteralInspector.GetAllStringLiterals(typeof(SqliteDbProvider).GetMethod("GetProductDetails")!);

            // Assert
            Assert.Contains("select * from Products where productCode = @productCode", literals);
            Assert.Contains("select * from Comments where productCode = @productCode", literals);
            Assert.DoesNotContain("select * from Products where productCode = '\" + productCode + \"'", string.Join("\n", literals));
            Assert.DoesNotContain("select * from Comments where productCode = '\" + productCode + \"'", string.Join("\n", literals));

            // Ensure relation name unchanged (behavioral integrity)
            Assert.Contains("prod_comments", literals);
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

                    if (op == 0x72) // ldstr
                    {
                        if (i + 4 > il.Length) break;
                        int token = BitConverter.ToInt32(il, i);
                        i += 4;
                        try { literals.Add(module.ResolveString(token)); } catch { }
                        continue;
                    }

                    // Operand sizes: use a simple heuristic sufficient for scanning.
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
