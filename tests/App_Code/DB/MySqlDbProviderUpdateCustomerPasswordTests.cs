using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedUpdate()
        {
            // Arrange
            string sql = "UPDATE CustomerLogin SET password = @password WHERE customerNumber = @customerNumber";

            // Assert
            Assert.Contains("@password", sql, StringComparison.Ordinal);
            Assert.Contains("@customerNumber", sql, StringComparison.Ordinal);
            Assert.DoesNotContain("set password = '\" +", sql, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("where customerNumber = \" +", sql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
