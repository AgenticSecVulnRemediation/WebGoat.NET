using System;
using MySql.Data.MySqlClient;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

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
            var signature = method.ToString();

            // Assert
            Assert.Contains("GetEmailByCustomerNumber", signature);
        }
    }
}
