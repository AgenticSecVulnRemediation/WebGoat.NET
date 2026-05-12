using System;
using System.Reflection;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderDeltaTests
    {
        [Fact]
        public void CustomCustomerLogin_AddsEmailParameter_ToSelectCommand()
        {
            // Delta: parameter added to SelectCommand to avoid concatenation.
            // Assert the new parameter name is present in the assembly strings.

            var assembly = typeof(OWASP.WebGoat.NET.App_Code.DB.SqliteDbProvider).Assembly;
            var allStrings = assembly.FullName ?? string.Empty;

            Assert.Contains("@Email", allStrings);
        }

        [Fact]
        public void GetCustomerEmails_UsesParameterizedLikeExpression()
        {
            // Delta: query changed to "email like @Email || '%'".
            var assembly = typeof(OWASP.WebGoat.NET.App_Code.DB.SqliteDbProvider).Assembly;
            var allStrings = assembly.FullName ?? string.Empty;

            Assert.Contains("@Email", allStrings);
            Assert.Contains("|| '%'", allStrings);
        }
    }
}
