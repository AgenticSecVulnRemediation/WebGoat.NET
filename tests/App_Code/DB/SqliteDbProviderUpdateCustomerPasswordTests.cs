using System;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedUpdate_PreventsSqlInjection()
        {
            // Arrange
            var sql = "UPDATE CustomerLogin SET password = @password WHERE customerNumber = @customerNumber";
            var password = "pw' OR 1=1 --";
            var customerNumber = 42;

            // Act
            using var conn = new SqliteConnection("Data Source=:memory:;Version=3");
            using var cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@customerNumber", customerNumber);

            // Assert
            Assert.Contains("@password", cmd.CommandText, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("@customerNumber", cmd.CommandText, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain(password, cmd.CommandText, StringComparison.Ordinal);
            Assert.Equal(password, cmd.Parameters["@password"].Value);
            Assert.Equal(customerNumber, cmd.Parameters["@customerNumber"].Value);
        }
    }
}
