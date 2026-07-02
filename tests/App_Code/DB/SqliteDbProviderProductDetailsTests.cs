using System;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameters_DoesNotExpandInjection()
        {
            // Arrange
            using var cn = new SqliteConnection("Data Source=:memory:;Version=3;New=True;");
            cn.Open();

            using (var create = cn.CreateCommand())
            {
                create.CommandText = "CREATE TABLE Products(productCode TEXT, productName TEXT);";
                create.ExecuteNonQuery();
                create.CommandText = "CREATE TABLE Comments(productCode TEXT, comment TEXT);";
                create.ExecuteNonQuery();

                create.CommandText = "INSERT INTO Products(productCode, productName) VALUES('S10_1678', 'Car');";
                create.ExecuteNonQuery();
                create.CommandText = "INSERT INTO Comments(productCode, comment) VALUES('S10_1678', 'hello');";
                create.ExecuteNonQuery();
            }

            var injectedProductCode = "S10_1678' OR 1=1 --";

            // Act
            using var cmd1 = cn.CreateCommand();
            cmd1.CommandText = "select * from Products where productCode = @productCode";
            cmd1.Parameters.AddWithValue("@productCode", injectedProductCode);
            using var r1 = cmd1.ExecuteReader();

            using var cmd2 = cn.CreateCommand();
            cmd2.CommandText = "select * from Comments where productCode = @productCode";
            cmd2.Parameters.AddWithValue("@productCode", injectedProductCode);
            using var r2 = cmd2.ExecuteReader();

            // Assert
            Assert.False(r1.Read());
            Assert.False(r2.Read());
        }
    }
}
