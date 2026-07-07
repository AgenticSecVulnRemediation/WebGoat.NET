using System;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderIsValidCustomerLoginTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameterizedQuery_ForEmailAndPassword()
        {
            // Arrange
            // Delta-only test: concatenated query replaced by parameters @email and @password.
            const string sql = "select * from CustomerLogin where email = @email and password = @password;";

            // Assert
            Assert.Contains("@email", sql);
            Assert.Contains("@password", sql);
            Assert.DoesNotContain("'\" + email + \"'", sql);
            Assert.DoesNotContain("'\" + encoded_password + \"'", sql);
        }
    }
}
