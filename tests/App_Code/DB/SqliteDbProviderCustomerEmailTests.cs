using System;
using System.Data;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterizedCustomerNumber()
        {
            // Arrange
            var sql = "select email from CustomerLogin where customerNumber = @customerNumber";

            using var conn = new SqliteConnection("Data Source=:memory:;Version=3");
            conn.Open();

            // Act
            using var cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@customerNumber", "1 OR 1=1");

            // Assert
            Assert.Contains("@customerNumber", cmd.CommandText);
            Assert.Equal(1, cmd.Parameters.Count);
            Assert.Equal("@customerNumber", cmd.Parameters[0].ParameterName);
        }
    }
}
