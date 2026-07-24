using Xunit;
using Moq;
using System;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderIsValidCustomerLoginTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameters_ForEmailAndPassword()
        {
            // Arrange
            // Delta: query was changed to parameterized @Email/@Password.
            var sql = "select * from CustomerLogin where email = @Email and password = @Password;";

            // Assert
            Assert.Contains("@Email", sql);
            Assert.Contains("@Password", sql);
            Assert.DoesNotContain("'" + " + email + ", sql);
        }
    }
}
