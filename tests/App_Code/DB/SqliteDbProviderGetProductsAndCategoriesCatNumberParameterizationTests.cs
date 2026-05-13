using System;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using Xunit;

// Assumption: Source namespace from file path is OWASP.WebGoat.NET.App_Code.DB
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductsAndCategoriesCatNumberParameterizationTests
    {
        [Fact]
        public void GetProductsAndCategories_WithCatNumber1_DoesNotThrowOnParameterBinding()
        {
            // Arrange
            var dbFile = Path.GetTempFileName();
            File.Delete(dbFile);
            SqliteConnection.CreateFile(dbFile);

            using (var cn = new SqliteConnection($"Data Source={dbFile};Version=3"))
            {
                cn.Open();
                using var cmd = cn.CreateCommand();
                cmd.CommandText = "CREATE TABLE Categories(catNumber INTEGER);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE TABLE Products(catNumber INTEGER);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO Categories(catNumber) VALUES(1);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO Products(catNumber) VALUES(1);";
                cmd.ExecuteNonQuery();
            }

            var config = new ConfigFile();
            config.Set(DbConstants.KEY_FILE_NAME, dbFile);
            config.Set(DbConstants.KEY_CLIENT_EXEC, "");
            var provider = new SqliteDbProvider(config);

            // Act
            var ex = Record.Exception(() => provider.GetProductsAndCategories(1));

            // Assert
            Assert.Null(ex);
        }
    }
}
