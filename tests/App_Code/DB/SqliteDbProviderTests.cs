using System;
using System.Reflection;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameters_ForEmailAndPassword()
        {
            // Arrange
            var method = typeof(SqliteDbProvider).GetMethod("IsValidCustomerLogin", BindingFlags.Public | BindingFlags.Instance);
            Assert.NotNull(method);

            const string expectedSql = "select * from CustomerLogin where email = @Email and password = @Password;";

            // Assert
            Assert.True(MethodContainsStringLiteral(method!, expectedSql),
                "Expected parameterized login query not found.");
            Assert.True(MethodContainsStringLiteral(method!, "@Email"));
            Assert.True(MethodContainsStringLiteral(method!, "@Password"));
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
