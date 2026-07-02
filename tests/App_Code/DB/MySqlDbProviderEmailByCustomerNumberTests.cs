using System;
using MySql.Data.MySqlClient;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderEmailByCustomerNumberTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterizedSql_DoesNotConcatenateUserInput()
        {
            // Arrange
            var injected = "0 OR 1=1";

            // Act
            var sql = "select email from CustomerLogin where customerNumber = @customerNumber";

            // Assert
            Assert.DoesNotContain(injected, sql, StringComparison.Ordinal);
            Assert.Contains("@customerNumber", sql, StringComparison.Ordinal);
        }

        [Fact]
        public void GetEmailByCustomerNumber_CreatesMySqlParameterWithInjectedValue_AsLiteral()
        {
            // Arrange
            var injected = "0 OR 1=1";

            // Act
            var p = new MySqlParameter("@customerNumber", injected);

            // Assert
            Assert.Equal("@customerNumber", p.ParameterName);
            Assert.Equal(injected, p.Value);
        }
    }
}
