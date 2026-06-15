using System;
using System.Reflection;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailsTests
    {
        [Fact]
        public void GetCustomerEmails_UsesParameterizedLikeQuery_AppendsWildcardInParameter()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("GetCustomerEmails", BindingFlags.Public | BindingFlags.Instance);
            Assert.NotNull(method);

            const string expectedSql = "select email from CustomerLogin where email like @email";

            // Assert
            Assert.True(MethodContainsString(method!, expectedSql),
                "Expected parameterized LIKE query not found.");
            Assert.True(MethodContainsString(method!, "email + \"%\""),
                "Expected wildcard concatenation in parameter value not found.");
        }

        private static bool MethodContainsString(MethodInfo method, string expected)
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
                        var s = module.ResolveString(token);
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
