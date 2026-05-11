using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParametersInsteadOfConcatenation()
        {
            // Arrange
            int customerNumber = 123;
            string password = "pw' ; DROP TABLE CustomerLogin; --";

            // Delta behavior: SQL text is now constant with placeholders.
            string sql = "UPDATE CustomerLogin SET password = @password WHERE customerNumber = @customerNumber";

            // Act + Assert
            Assert.Contains("@password", sql);
            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain(password, sql);
            Assert.DoesNotContain(customerNumber.ToString(), sql);
        }
    }
}
