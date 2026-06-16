using System;
using System.Reflection;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPasswordByEmailTests
    {
        [Fact]
        public void GetPasswordByEmail_UsesParameterizedQuery_ContainsEmailParameterPlaceholder()
        {
            // Arrange
            // Delta test for GetPasswordByEmail(): query changed from string concatenation to parameterized MySqlCommand.
            // We verify the parameter placeholder "@email" is present in the SQL string literal.

            var method = typeof(MySqlDbProvider).GetMethod("GetPasswordByEmail");
            Assert.NotNull(method);

            // Act
            var body = method!.GetMethodBody();
            Assert.NotNull(body);
            var il = body!.GetILAsByteArray();
            var module = typeof(MySqlDbProvider).Module;

            // Assert
            bool found = false;
            for (int i = 0; i < il.Length - 4; i++)
            {
                if (il[i] == 0x72)
                {
                    int token = BitConverter.ToInt32(il, i + 1);
                    string s;
                    try { s = module.ResolveString(token); }
                    catch { continue; }
                    if (s.Contains("select * from CustomerLogin where email = @email"))
                    {
                        found = true;
                        break;
                    }
                }
            }

            Assert.True(found, "Expected parameterized SELECT statement string literal not found.");
        }
    }
}
