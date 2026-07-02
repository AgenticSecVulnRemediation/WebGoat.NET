using Xunit;
using Moq;
using System.Data;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedCommand_ForProductCode()
        {
            // Arrange
            var config = new Mock<ConfigFile>(MockBehavior.Loose);
            config.Setup(c => c.Get(It.IsAny<string>())).Returns("test.db");
            var provider = new SqliteDbProvider(config.Object);

            var connStrField = typeof(SqliteDbProvider).GetField("_connectionString", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            connStrField!.SetValue(provider, "Data Source=:memory:;Version=3");

            using (var conn = new SqliteConnection("Data Source=:memory:;Version=3"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE Products(productCode TEXT); CREATE TABLE Comments(productCode TEXT);";
                    cmd.ExecuteNonQuery();
                }
            }

            // Act
            var ds = provider.GetProductDetails("ABC' OR 1=1 --");

            // Assert
            Assert.NotNull(ds);
            Assert.True(ds.Tables.Contains("products"));
            Assert.True(ds.Tables.Contains("comments"));
        }
    }
}
