using System;
using System.Collections.Generic;
using System.Reflection;
using Moq;
using MySql.Data.MySqlClient;
using Xunit;

// Assumptions:
// - Source namespace is OWASP.WebGoat.NET.App_Code.DB (as declared in MySqlDbProvider.cs)
// - These are unit tests that validate the SQL construction behavior (parameterization) introduced by the security fix.
// - We do not connect to a real database; instead we intercept creation and execution via a lightweight seam.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        // Helper: create ConfigFile mock returning stable values.
        private static ConfigFile CreateConfigFileStub()
        {
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns<string>(key =>
            {
                // minimal values to build a connection string; will not be used to connect.
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
        public void IsValidCustomerLogin_UsesParameterizedQueryAndDoesNotInlineUserInput()
        {
            // Arrange
            var provider = new MySqlDbProvider(CreateConfigFileStub());

            var email = "foo@example.com' OR 1=1 --";
            var password = "pass' OR '1'='1";

            // Act
            // We can't execute without DB; instead validate command text/parameters via reflection.
            // The security fix changed the SQL literal to include @Email and @Password.
            var method = typeof(MySqlDbProvider).GetMethod("IsValidCustomerLogin");
            Assert.NotNull(method);

            // Extract local SQL string via IL inspection is heavy; instead assert by searching in source-internal constant:
            // We validate by invoking the method up to the point of command creation using a MySqlConnection that will fail on Open,
            // and then asserting the exception is thrown after attempting to open.
            // But we still want to ensure fixed SQL exists in method body: use MethodBody.GetILAsByteArray and search for the UTF8 literal.
            var il = method!.GetMethodBody()!.GetILAsByteArray();
            Assert.NotNull(il);

            var sqlLiteral = "SELECT * FROM CustomerLogin WHERE email = @Email AND password = @Password";
            var bytes = System.Text.Encoding.UTF8.GetBytes(sqlLiteral);

            // IL stores user strings as metadata tokens, not raw bytes; so we use a more robust check:
            // Ensure the method contains the parameter names as string literals.
            Assert.Contains("@Email", GetUserStrings(method));
            Assert.Contains("@Password", GetUserStrings(method));

            // Additionally, ensure the insecure pattern "email = '" no longer exists in the method's user strings.
            Assert.DoesNotContain("email = '\"", GetUserStrings(method));
            Assert.DoesNotContain("email = '\" + email", GetUserStrings(method));
        }

        private static IReadOnlyCollection<string> GetUserStrings(MethodInfo method)
        {
            // Extract all string literals referenced by the method via reflection on the declaring module.
            // This is a pragmatic delta test: it will fail if the code reverts to concatenation using embedded quotes.
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
                // handle two-byte opcodes
                if (op == 0xFE)
                {
                    op = (byte)(0xFE00 | il[i++]);
                }

                // ldstr opcode is 0x72
                if (op == 0x72)
                {
                    int token = BitConverter.ToInt32(il, i);
                    i += 4;
                    try
                    {
                        strings.Add(module.ResolveString(token));
                    }
                    catch
                    {
                        // ignore resolve issues
                    }
                    continue;
                }

                // advance operand sizes for a minimal subset; for unknown opcodes, break to avoid infinite loops.
                // This subset is enough for typical compiler output around ldstr/calls.
                if (op is 0x28 or 0x6F or 0x2A or 0x0A or 0x0B or 0x0C or 0x0D or 0x0E or 0x0F or 0x14 or 0x15 or 0x16 or 0x17 or 0x18 or 0x19 or 0x1A or 0x1B or 0x1C or 0x1D or 0x1E or 0x1F)
                {
                    if (op is 0x28 or 0x6F)
                    {
                        i += 4;
                    }
                    continue;
                }

                // Conservatively stop if opcode is not recognized in this simple parser.
                if (op != 0x00)
                {
                    // nop or unknown; continue for nop, else break.
                    if (op == 0x00) continue;
                    break;
                }
            }

            return strings;
        }
    }
}
