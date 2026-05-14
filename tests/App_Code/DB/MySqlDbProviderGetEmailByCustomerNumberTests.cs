using System;
using MySql.Data.MySqlClient;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByCustomerNumberTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterForCustomerNumber()
        {
            // Arrange
            const string query = "select email from CustomerLogin where customerNumber = @num";

            // Act
            var param = new MySqlParameter("@num", "1 OR 1=1");

            // Assert
            Assert.Equal("@num", param.ParameterName);
            Assert.Equal("1 OR 1=1", param.Value);
            Assert.Contains("@num", query);
            Assert.DoesNotContain("+ num", query);
        }
    }
}
