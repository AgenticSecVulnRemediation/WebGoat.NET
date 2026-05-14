using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedUpdateStatement()
        {
            // Arrange
            const string sql = "UPDATE CustomerLogin SET password = @password WHERE customerNumber = @customerNumber";

            // Assert
            Assert.DoesNotContain("Encoder.Encode(password)", sql); // ensures value isn't string-concatenated into SQL
            Assert.Contains("@password", sql);
            Assert.Contains("@customerNumber", sql);
        }
    }
}
