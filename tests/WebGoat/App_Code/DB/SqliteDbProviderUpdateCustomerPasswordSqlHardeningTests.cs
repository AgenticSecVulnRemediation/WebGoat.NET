using System;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordSqlHardeningTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedSql_DoesNotInlinePasswordOrCustomerNumber()
        {
            // Arrange
            var sql = "UPDATE CustomerLogin SET password = @password WHERE customerNumber = @customerNumber";
            var password = "p@ss'word";
            var customerNumber = 42;

            // Act
            var cmd = new SqliteCommand();
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@customerNumber", customerNumber);

            // Assert
            Assert.Contains("@password", cmd.CommandText);
            Assert.Contains("@customerNumber", cmd.CommandText);
            Assert.DoesNotContain(password, cmd.CommandText);
            Assert.DoesNotContain(customerNumber.ToString(), cmd.CommandText);

            Assert.Equal(2, cmd.Parameters.Count);
            Assert.NotNull(cmd.Parameters["@password"]);
            Assert.NotNull(cmd.Parameters["@customerNumber"]);
        }
    }
}
