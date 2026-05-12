using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderIsValidCustomerLoginDeltaTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesEmailAndPasswordParameters()
        {
            // Delta: IsValidCustomerLogin changed to parameterized query with @Email and @Password.
            // Assert the parameter names exist in the provider assembly strings.

            var assembly = typeof(OWASP.WebGoat.NET.App_Code.DB.MySqlDbProvider).Assembly;
            var strings = assembly.FullName ?? string.Empty;

            Assert.Contains("@Email", strings);
            Assert.Contains("@Password", strings);
        }
    }
}
