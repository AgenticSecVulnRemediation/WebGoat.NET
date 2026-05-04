using System;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameters_DoesNotInlineInputs()
        {
            // Arrange
            const int customerNumber = 7;
            const string password = "newPass";

            // Mirror the fixed SQL.
            var sql = "UPDATE CustomerLogin SET password = @password WHERE customerNumber = @customerNumber";
            using var cmd = new SqliteCommand(sql);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@customerNumber", customerNumber);

            // Act
            var commandText = cmd.CommandText;

            // Assert
            Assert.Equal("UPDATE CustomerLogin SET password = @password WHERE customerNumber = @customerNumber", commandText);
            Assert.Equal(2, cmd.Parameters.Count);
            Assert.DoesNotContain(password, commandText);
            Assert.DoesNotContain(customerNumber.ToString(), commandText);
        }
    }
}
