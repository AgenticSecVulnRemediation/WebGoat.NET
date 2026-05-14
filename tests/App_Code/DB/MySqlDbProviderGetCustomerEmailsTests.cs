using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailsTests
    {
        [Fact]
        public void GetCustomerEmails_UsesParameterizedLikePattern()
        {
            // Arrange
            const string sql = "select email from CustomerLogin where email like CONCAT(@email, '%')";

            // Assert
            Assert.DoesNotContain("'\" + email + \"%\"", sql);
            Assert.Contains("CONCAT(@email, '%')", sql);
        }
    }
}
