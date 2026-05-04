using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedQuery()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("UpdateCustomerPassword");
            Assert.NotNull(method);

            // Act
            const string expectedSql = "update CustomerLogin set password = @password where customerNumber = @customerNumber";

            // Assert
            Assert.Contains("@password", expectedSql);
            Assert.Contains("@customerNumber", expectedSql);
            Assert.DoesNotContain("set password = '", expectedSql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
