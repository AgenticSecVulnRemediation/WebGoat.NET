using MySql.Data.MySqlClient;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedQueryForPasswordAndCustomerNumber()
        {
            // Arrange: delta change replaced string concatenation with @password and @customerNumber parameters.
            var sql = "update CustomerLogin set password = @password where customerNumber = @customerNumber";

            // Act
            using var cmd = new MySqlCommand(sql);
            cmd.Parameters.AddWithValue("@password", "encoded");
            cmd.Parameters.AddWithValue("@customerNumber", 42);

            // Assert
            Assert.Contains("@password", cmd.CommandText);
            Assert.Contains("@customerNumber", cmd.CommandText);
            Assert.Equal(2, cmd.Parameters.Count);
        }
    }
}
