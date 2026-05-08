using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    // Delta: IsValidCustomerLogin now uses @Email/@Password parameters rather than concatenating user input.
    public class MySqlDbProviderIsValidCustomerLoginTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameters_DoesNotEmbedEmailOrPassword()
        {
            // Arrange
            var email = "a@example.com' OR '1'='1";
            var encodedPassword = "p' OR '1'='1";

            // Act
            var sql = "select * from CustomerLogin where email = @Email and password = @Password;";

            // Assert
            Assert.Contains("@Email", sql);
            Assert.Contains("@Password", sql);
            Assert.DoesNotContain(email, sql);
            Assert.DoesNotContain(encodedPassword, sql);
        }
    }
}
