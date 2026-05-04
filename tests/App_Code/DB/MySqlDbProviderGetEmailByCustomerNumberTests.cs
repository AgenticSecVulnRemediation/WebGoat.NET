using System;
using MySql.Data.MySqlClient;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByCustomerNumberTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterizedQuery()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("GetEmailByCustomerNumber");
            Assert.NotNull(method);

            // Act
            const string expectedQuery = "select email from CustomerLogin where customerNumber = @CustomerNumber";

            // Assert
            Assert.Contains("@CustomerNumber", expectedQuery);
            Assert.DoesNotContain("customerNumber = ", expectedQuery.Replace("@CustomerNumber", ""), StringComparison.OrdinalIgnoreCase);
        }
    }
}
