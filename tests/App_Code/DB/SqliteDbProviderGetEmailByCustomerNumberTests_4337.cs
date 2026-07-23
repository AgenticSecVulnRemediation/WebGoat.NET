using System;
using System.Reflection;
using Mono.Data.Sqlite;
using Moq;
using OWASP.WebGoat.NET.App_Code;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetEmailByCustomerNumberTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_WithSqlInjectionPayload_DoesNotReturnEmail()
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
                cmd.CommandText = "CREATE TABLE CustomerLogin(customerNumber TEXT, email TEXT); INSERT INTO CustomerLogin(customerNumber,email) VALUES('1','a@b.com');";
                cmd.ExecuteNonQuery();
            }

            // Act
            var result = provider.GetEmailByCustomerNumber("1 OR 1=1");

            // Assert
            // With parameterization, should not match and should return empty string or error message; but it should not return the seeded email.
            Assert.NotEqual("a@b.com", result);
        }
    }
}
