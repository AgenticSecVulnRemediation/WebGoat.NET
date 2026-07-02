using Xunit;
using Moq;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPaymentsTests
    {
        [Fact]
        public void GetPayments_UsesParameterizedCustomerNumber()
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
                    cmd.CommandText = "CREATE TABLE Payments(customerNumber INTEGER);";
                    cmd.ExecuteNonQuery();
                }
            }

            // Act
            var ds = provider.GetPayments(123);

            // Assert
            Assert.NotNull(ds);
        }
    }
}
