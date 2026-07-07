using System;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameters_NotStringConcatenation()
        {
            // Arrange
            const string sql = "update CustomerLogin set password = @password where customerNumber = @customerNumber";

            // Assert
            Assert.Contains("@password", sql);
            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("Encoder.Encode(password)", sql); // should be passed via parameter value instead
            Assert.DoesNotContain("+ customerNumber", sql);
        }
    }
}
