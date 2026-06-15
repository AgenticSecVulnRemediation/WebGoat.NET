using System;
using System.Reflection;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsert_ForAllUserInput()
        {
            // Arrange
            var method = typeof(SqliteDbProvider).GetMethod("AddComment", BindingFlags.Public | BindingFlags.Instance);
            Assert.NotNull(method);

            const string expectedSql = "insert into Comments(productCode, email, comment) values (@productCode, @email, @comment);";

            // Assert
            Assert.True(MethodContainsStringLiteral(method!, expectedSql),
                "Expected parameterized insert statement not found.");
            Assert.True(MethodContainsStringLiteral(method!, "@productCode"));
            Assert.True(MethodContainsStringLiteral(method!, "@email"));
            Assert.True(MethodContainsStringLiteral(method!, "@comment"));
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
