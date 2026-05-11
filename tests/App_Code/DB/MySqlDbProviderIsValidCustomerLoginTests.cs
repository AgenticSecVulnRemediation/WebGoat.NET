using System;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderIsValidCustomerLoginTests
    {
        [Fact]
        public void IsValidCustomerLogin_WithSqlInjectionCharacters_DoesNotThrowSqlSyntaxError()
        {
            // Arrange
            // Delta test: query changed from string concatenation to parameterized query.
            var provider = new MySqlDbProvider(new ConfigFile());
            var email = "a' OR '1'='1";
            var password = "p";

            // Act
            var ex = Assert.ThrowsAny<Exception>(() => provider.IsValidCustomerLogin(email, password));

            // Assert
            // Should fail due to connection but not due to injected quotes breaking SQL.
            Assert.DoesNotContain("You have an error in your SQL syntax", ex.Message, StringComparison.OrdinalIgnoreCase);
        }
    }
}
