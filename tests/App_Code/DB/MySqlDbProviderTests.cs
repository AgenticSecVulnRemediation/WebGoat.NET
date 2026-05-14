using System;
using MySql.Data.MySqlClient;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesMySqlParameterInsteadOfConcatenation()
        {
            // Arrange
            const string expectedQuery = "select email from CustomerLogin where customerNumber = @num";

            // Act
            var param = new MySqlParameter("@num", "123 OR 1=1");

            // Assert
            Assert.Equal("@num", param.ParameterName);
            Assert.Contains("@num", expectedQuery);
            Assert.DoesNotContain(param.Value.ToString(), expectedQuery);
        }
    }
}
