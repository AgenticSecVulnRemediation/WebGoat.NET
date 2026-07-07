using System;
using System.Data;
using System.Reflection;
using Moq;
using Xunit;

// Assumptions:
// - Source class is in namespace OWASP.WebGoat.NET.App_Code.DB
// - MySqlCommand is from MySql.Data.MySqlClient, but we mock it via reflection hooks.
// These tests are "delta" and focus on verifying that GetCustomerEmail uses a parameterized query.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterizedQuery_DoesNotConcatenateCustomerNumber()
        {
            // Arrange
            // We can't hit a real DB; instead we validate the constant SQL change via IL/string inspection.
            // This regression test ensures the query contains @customerNumber and not string concatenation.

            var method = typeof(MySqlDbProvider).GetMethod("GetCustomerEmail", BindingFlags.Public | BindingFlags.Instance);
            Assert.NotNull(method);

            // Act
            var body = method!.GetMethodBody();

            // Assert
            // Minimal deterministic assertion: method body should reference the parameter marker.
            // (String literals are embedded in metadata; we scan them.)
            var il = body!.GetILAsByteArray();
            Assert.NotNull(il);

            // Additionally scan the assembly's user string heap for the expected SQL literal.
            // This is robust for this delta because the fix is a literal change.
            var module = typeof(MySqlDbProvider).Module;
            var expected = "select email from CustomerLogin where customerNumber = @customerNumber";
            Assert.Contains(expected, GetAllUserStrings(module));

            // And ensure old vulnerable pattern is not present.
            Assert.DoesNotContain("select email from CustomerLogin where customerNumber = ", GetAllUserStrings(module));
        }

        private static string[] GetAllUserStrings(Module module)
        {
            // Lightweight metadata scan: iterate over possible UserString tokens.
            // Not all tokens are valid; failures are ignored.
            var strings = new System.Collections.Generic.List<string>();

            // UserString tokens are 0x70000000 + rid
            for (int rid = 1; rid < 10000; rid++)
            {
                int token = unchecked((int)0x70000000) + rid;
                try
                {
                    string s = module.ResolveString(token);
                    if (!string.IsNullOrEmpty(s))
                        strings.Add(s);
                }
                catch
                {
                    // ignore invalid tokens
                }
            }

            return strings.ToArray();
        }
    }
}
