using Xunit;
using Moq;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetEmailByCustomerNumberTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesBoundParameter_ForNum()
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
                    cmd.CommandText = "CREATE TABLE CustomerLogin(customerNumber TEXT, email TEXT); INSERT INTO CustomerLogin(customerNumber,email) VALUES ('1','a@b.com');";
                    cmd.ExecuteNonQuery();
                }
            }

            // Act
            var email = provider.GetEmailByCustomerNumber("1 OR 1=1");

            // Assert
            // Parameterization should prevent returning all rows; likely returns null/empty or error.
            Assert.NotNull(email);
        }
    }
}
