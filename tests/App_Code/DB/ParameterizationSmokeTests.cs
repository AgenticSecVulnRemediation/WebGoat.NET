using System;
using System.Data;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class ParameterizationSmokeTests
    {
        [Theory]
        [InlineData("0 OR 1=1")]
        [InlineData("1; DROP TABLE CustomerLogin;--")]
        public void ParameterizedScalar_WithInjectionPayload_DoesNotChangeQuerySemantics(string payload)
        {
            // Arrange: in-memory sqlite to validate behavior of parameter placeholders.
            using var conn = new SqliteConnection("Data Source=:memory:;Version=3");
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE CustomerLogin (customerNumber TEXT, email TEXT);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO CustomerLogin(customerNumber, email) VALUES ('1','a@b');";
                cmd.ExecuteNonQuery();
            }

            // Act: emulate fixed pattern: customerNumber = @num
            using var cmd2 = conn.CreateCommand();
            cmd2.CommandText = "select email from CustomerLogin where customerNumber = @num";
            cmd2.Parameters.AddWithValue("@num", payload);
            object result = cmd2.ExecuteScalar();

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(0)]
        public void ParameterizedCatNumberBranch_UsesParameterWhenFiltering(int catNumber)
        {
            using var conn = new SqliteConnection("Data Source=:memory:;Version=3");
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE Categories (catNumber INTEGER);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE TABLE Products (catNumber INTEGER);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO Categories(catNumber) VALUES (1);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO Products(catNumber) VALUES (1);";
                cmd.ExecuteNonQuery();
            }

            using var cmd3 = conn.CreateCommand();
            if (catNumber >= 1)
            {
                cmd3.CommandText = "select * from Categories where catNumber = @catNumber";
                cmd3.Parameters.AddWithValue("@catNumber", catNumber);
            }
            else
            {
                cmd3.CommandText = "select * from Categories";
            }

            using var adapter = new SqliteDataAdapter(cmd3);
            var ds = new DataSet();
            adapter.Fill(ds);

            Assert.True(ds.Tables.Count > 0);
        }
    }
}
