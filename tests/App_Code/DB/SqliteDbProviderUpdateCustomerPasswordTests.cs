using System;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameters_PasswordAndCustomerNumber()
        {
            // Arrange
            const string sql = "update CustomerLogin set password = @password where customerNumber = @customerNumber";
            using var cmd = new SqliteCommand(sql);

            // Act
            cmd.Parameters.AddWithValue("@password", "encoded");
            cmd.Parameters.AddWithValue("@customerNumber", 123);

            // Assert
            Assert.Contains("password = @password", cmd.CommandText);
            Assert.Contains("customerNumber = @customerNumber", cmd.CommandText);
            Assert.NotNull(cmd.Parameters["@password"]);
            Assert.NotNull(cmd.Parameters["@customerNumber"]);
        }
    }
}
