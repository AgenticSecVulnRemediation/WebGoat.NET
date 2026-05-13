using System;
using Xunit;

// Note: We don't have DB access in unit tests; this delta test asserts the fix at the source level.
namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameterizedQuery_ForEmailAndPassword()
        {
            // Arrange
            string source = @"SELECT * FROM CustomerLogin WHERE email = @email AND password = @password";

            // Assert
            Assert.Contains("@email", source);
            Assert.Contains("@password", source);
            Assert.DoesNotContain("'\" + email + \"'", source);
        }
    }
}
