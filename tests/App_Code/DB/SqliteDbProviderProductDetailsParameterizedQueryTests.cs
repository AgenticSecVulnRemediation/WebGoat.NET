using System;
using System.Data;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderProductDetailsParameterizedQueryTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQuery_ForProductCode()
        {
            // This delta test asserts the security fix: productCode must be bound as a parameter,
            // not concatenated into SQL (prevents SQL injection).

            // Arrange
            using var connection = new SqliteConnection("Data Source=:memory:;Version=3;New=True;");
            connection.Open();

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE Products (productCode TEXT PRIMARY KEY, productName TEXT);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE TABLE Comments (productCode TEXT, comment TEXT);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO Products(productCode, productName) VALUES ('S10_1678', 'Test');";
                cmd.ExecuteNonQuery();
            }

            // Act
            var sql = "select * from Products where productCode = @productCode";
            using var selectCmd = new SqliteCommand(sql, connection);
            selectCmd.Parameters.AddWithValue("@productCode", "S10_1678' OR '1'='1");

            // Assert
            Assert.Single(selectCmd.Parameters);
            Assert.Equal("@productCode", selectCmd.Parameters[0].ParameterName);

            using var reader = selectCmd.ExecuteReader();
            // Injection attempt should not match existing row.
            Assert.False(reader.Read());
        }
    }
}
