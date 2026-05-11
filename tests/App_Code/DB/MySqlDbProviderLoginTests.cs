using System;
using System.Data;
using MySql.Data.MySqlClient;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderLoginTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameters_EmailAndPasswordNotConcatenated()
        {
            // Arrange
            var email = "a@b.com' OR '1'='1";
            var encodedPassword = "pw' OR '1'='1";

            using var conn = new MySqlConnection();
            var sql = "select * from CustomerLogin where email = @Email and password = @Password;";

            // Act
            var da = new MySqlDataAdapter(sql, conn);
            da.SelectCommand.Parameters.AddWithValue("@Email", email);
            da.SelectCommand.Parameters.AddWithValue("@Password", encodedPassword);

            // Assert
            Assert.Contains("email = @Email", da.SelectCommand.CommandText, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("password = @Password", da.SelectCommand.CommandText, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain(email, da.SelectCommand.CommandText, StringComparison.Ordinal);
            Assert.DoesNotContain(encodedPassword, da.SelectCommand.CommandText, StringComparison.Ordinal);
            Assert.Equal(email, da.SelectCommand.Parameters["@Email"].Value);
            Assert.Equal(encodedPassword, da.SelectCommand.Parameters["@Password"].Value);
        }
    }
}
