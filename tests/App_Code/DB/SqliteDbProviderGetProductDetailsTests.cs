using System;
using System.Reflection;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQueries_ForProductCode()
        {
            // Arrange
            var method = typeof(SqliteDbProvider).GetMethod("GetProductDetails", BindingFlags.Public | BindingFlags.Instance);
            Assert.NotNull(method);

            const string expectedProductsSql = "select * from Products where productCode = @productCode";
            const string expectedCommentsSql = "select * from Comments where productCode = @productCode";

            // Assert
            Assert.True(MethodContainsStringLiteral(method!, expectedProductsSql),
                "Expected parameterized Products query not found.");
            Assert.True(MethodContainsStringLiteral(method!, expectedCommentsSql),
                "Expected parameterized Comments query not found.");
            Assert.True(MethodContainsStringLiteral(method!, "@productCode"));
        }

        private static bool MethodContainsStringLiteral(MethodInfo method, string expected)
        {
            var il = method.GetMethodBody()?.GetILAsByteArray();
            if (il == null) return false;

            var module = method.Module;
            for (int i = 0; i < il.Length - 4; i++)
            {
                if (il[i] == 0x72)
                {
                    int token = BitConverter.ToInt32(il, i + 1);
                    try
                    {
                        string s = module.ResolveString(token);
                        if (s == expected) return true;
                        if (s.Contains(expected)) return true;
                    }
                    catch
                    {
                        // ignore
                    }
                }
            }
            return false;
        }
    }
}
