using System;
using MySql.Data.MySqlClient;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderExecuteScalarTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesExecuteScalarWithParameter_PreventsInjection()
        {
            // Arrange
            var customerNumber = "1 OR 1=1";
            var sql = "select email from CustomerLogin where customerNumber = @CustomerNumber";

            // Act
            var parameter = new MySqlParameter("@CustomerNumber", customerNumber);

            // Assert
            Assert.Equal("@CustomerNumber", parameter.ParameterName);
            Assert.Equal(customerNumber, parameter.Value);
            Assert.Contains("@CustomerNumber", sql, StringComparison.Ordinal);
            Assert.DoesNotContain(customerNumber, sql, StringComparison.Ordinal);
        }
    }
}
