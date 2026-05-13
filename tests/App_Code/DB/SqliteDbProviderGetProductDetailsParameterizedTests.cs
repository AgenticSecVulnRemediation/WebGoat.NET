using System;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using Xunit;

// Assumption: Source namespace from file path is OWASP.WebGoat.NET.App_Code.DB
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsParameterizedTests
    {
        [Fact]
        public void GetProductDetails_WithQuoteInProductCode_DoesNotThrowSqlSyntaxError()
        {
            // Arrange
            // Create temp sqlite db file with required schema.
            var dbFile = Path.GetTempFileName();
            File.Delete(dbFile);
            SqliteConnection.CreateFile(dbFile);

            using (var cn = new SqliteConnection($"Data Source={dbFile};Version=3"))
            {
                cn.Open();
                using var cmd = cn.CreateCommand();
                cmd.CommandText = "CREATE TABLE Products(productCode TEXT PRIMARY KEY);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE TABLE Comments(productCode TEXT, email TEXT, comment TEXT);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO Products(productCode) VALUES(@pc);";
                cmd.Parameters.AddWithValue("@pc", "p1");
                cmd.ExecuteNonQuery();
            }

            var config = new ConfigFile();
            config.Set(DbConstants.KEY_FILE_NAME, dbFile);
            config.Set(DbConstants.KEY_CLIENT_EXEC, "");
            var provider = new SqliteDbProvider(config);

            // Act
            var ex = Record.Exception(() => provider.GetProductDetails("p1' OR 1=1 --"));

            // Assert
            Assert.Null(ex);
        }
    }
}
