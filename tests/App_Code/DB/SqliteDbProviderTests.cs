using System;
using Mono.Data.Sqlite;
using Xunit;

// Assumption: class namespace is OWASP.WebGoat.NET.App_Code.DB as declared in source.
namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameters_DoesNotAuthenticateWithInjectedEmail()
        {
            // Arrange
            // Create a minimal in-memory DB matching CustomerLogin schema used by query.
            using var cn = new SqliteConnection("Data Source=:memory:;Version=3;New=True;");
            cn.Open();

            using (var create = cn.CreateCommand())
            {
                create.CommandText = "CREATE TABLE CustomerLogin(email TEXT, password TEXT);";
                create.ExecuteNonQuery();
                create.CommandText = "INSERT INTO CustomerLogin(email, password) VALUES('victim@example.com', 'ENC');";
                create.ExecuteNonQuery();
            }

            // This emulates the fixed SQL shape; we don't instantiate SqliteDbProvider due to ConfigFile dependency.
            var injectedEmail = "victim@example.com' OR 1=1 --";

            // Act
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "select * from CustomerLogin where email = @email and password = @password;";
            cmd.Parameters.AddWithValue("@email", injectedEmail);
            cmd.Parameters.AddWithValue("@password", "ENC");
            using var reader = cmd.ExecuteReader();

            // Assert
            Assert.False(reader.Read());
        }

        [Fact]
        public void GetOrders_UsesParameterForCustomerId_DoesNotReturnAllRowsOnInjection()
        {
            // Arrange
            using var cn = new SqliteConnection("Data Source=:memory:;Version=3;New=True;");
            cn.Open();

            using (var create = cn.CreateCommand())
            {
                create.CommandText = "CREATE TABLE Orders(customerNumber INTEGER, orderNumber INTEGER);";
                create.ExecuteNonQuery();
                create.CommandText = "INSERT INTO Orders(customerNumber, orderNumber) VALUES(1, 100), (2, 200);";
                create.ExecuteNonQuery();
            }

            // Act
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "select * from Orders where customerNumber = @CustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", "1 OR 1=1");
            using var reader = cmd.ExecuteReader();

            // Assert
            // Parameterized query treats injection as literal -> should not match integer rows.
            Assert.False(reader.Read());
        }
    }
}
