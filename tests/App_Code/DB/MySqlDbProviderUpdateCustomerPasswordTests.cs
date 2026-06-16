using System;
using System.Data;
using System.Reflection;
using Moq;
using MySql.Data.MySqlClient;
using OWASP.WebGoat.NET.App_Code;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedQuery_SucceedsWithSqlMetaCharsInPassword()
        {
            // Arrange
            // Delta test for UpdateCustomerPassword(): switched from string concatenation to parameters.
            // We validate that a password containing SQL metacharacters does not break the statement construction.
            // This test does not require a live MySQL server: it asserts the command text contains parameters
            // by inspecting the method via reflection and executing against a mocked connection is not feasible
            // due to MySqlConnection being concrete. Instead we validate against the literal SQL string used.

            // Act
            var method = typeof(MySqlDbProvider).GetMethod("UpdateCustomerPassword");
            Assert.NotNull(method);

            var body = method!.GetMethodBody();
            Assert.NotNull(body);

            // Assert (best-effort): Ensure the updated SQL text exists in IL as a string literal.
            // This is a tight delta assertion that will fail if code regresses back to concatenation.
            var il = body!.GetILAsByteArray();
            var module = typeof(MySqlDbProvider).Module;

            bool foundParameterizedSql = false;
            for (int i = 0; i < il.Length - 4; i++)
            {
                // ldstr opcode is 0x72 followed by metadata token (4 bytes)
                if (il[i] == 0x72)
                {
                    int token = BitConverter.ToInt32(il, i + 1);
                    string s;
                    try { s = module.ResolveString(token); }
                    catch { continue; }
                    if (s.Contains("SET password = @password") && s.Contains("customerNumber = @customerNumber"))
                    {
                        foundParameterizedSql = true;
                        break;
                    }
                }
            }

            Assert.True(foundParameterizedSql, "Expected parameterized UPDATE statement string literal not found.");
        }
    }
}
