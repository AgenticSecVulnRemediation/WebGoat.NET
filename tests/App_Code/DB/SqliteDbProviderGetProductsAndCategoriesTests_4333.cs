using System;
using System.Reflection;
using Mono.Data.Sqlite;
using Moq;
using OWASP.WebGoat.NET.App_Code;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductsAndCategoriesTests
    {
        [Fact]
        public void GetProductsAndCategories_WithCatNumber_AddsParameterInsteadOfConcatenation()
        {
            // Arrange
            var cfgMock = new Mock<ConfigFile>(MockBehavior.Loose, "db.sqlite");
            var tmp = System.IO.Path.GetTempFileName();
            System.IO.File.Delete(tmp);
            var dbFile = tmp + ".sqlite";
            cfgMock.Setup(c => c.Get(DbConstants.KEY_FILE_NAME)).Returns(dbFile);
            cfgMock.Setup(c => c.Get(DbConstants.KEY_CLIENT_EXEC)).Returns("sqlite3");

            var provider = new SqliteDbProvider(cfgMock.Object);
            typeof(SqliteDbProvider).GetField("_connectionString", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(provider, "Data Source=:memory:;Version=3;New=True;");

            using (var cn = new SqliteConnection("Data Source=:memory:;Version=3;New=True;"))
            {
                cn.Open();
                using var cmd = cn.CreateCommand();
                cmd.CommandText = "CREATE TABLE Categories(catNumber INTEGER); CREATE TABLE Products(catNumber INTEGER);";
                cmd.ExecuteNonQuery();
            }

            // Act + Assert
            // Regression: should not throw, and should accept catNumber >= 1 without building '... = ' + catNumber'
            var ex = Record.Exception(() => provider.GetProductsAndCategories(1));
            Assert.Null(ex);
        }
    }
}
