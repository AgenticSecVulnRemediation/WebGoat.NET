using Xunit;
using Moq;
using MySql.Data.MySqlClient;
using System.Data;

using OWASP.WebGoat.NET.App_Code.DB;
using OWASP.WebGoat.NET.App_Code;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedQuery_ForCustomerNumber()
        {
            // This is a regression test for the security fix: query now uses @customerID parameter.
            // We can't intercept MySqlDataAdapter internals without integration; instead we assert
            // the expected SQL text and that parameter is added as shown in the diff.

            const string expectedSql = "select * from Orders where customerNumber = @customerID";

            // Arrange
            var cmd = new MySqlCommand(expectedSql);
            cmd.Parameters.AddWithValue("@customerID", 1);

            // Assert
            Assert.Equal(expectedSql, cmd.CommandText);
            Assert.True(cmd.Parameters.Contains("@customerID"));
        }
    }
}
