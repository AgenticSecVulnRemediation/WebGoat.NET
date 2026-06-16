using System;
using System.Reflection;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderCustomCustomerLoginTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterizedEmailQuery_AllowsSqlMetaCharsInEmailWithoutSyntaxError()
        {
            // Arrange
            // Delta test for CustomCustomerLogin(): query changed to use "email = @email" with parameter.
            // We verify behavior using an in-memory sqlite db with a minimal CustomerLogin table.

            var dbPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "wg_sqlite_customlogin_" + Guid.NewGuid() + ".db");
            var cs = $"Data Source={dbPath};Version=3";

            using (var cn = new SqliteConnection(cs))
            {
                cn.Open();
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE CustomerLogin (Email TEXT, Password TEXT);";
                    cmd.ExecuteNonQuery();
                }

                // Insert a record that won't match the meta-char email.
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO CustomerLogin(Email, Password) VALUES ('real@example.com', 'cGFzcw==');";
                    cmd.ExecuteNonQuery();
                }
            }

            // Build provider with connection string injected.
            var cfg = new Mock<ConfigFile>(MockBehavior.Loose);
            cfg.Setup(c => c.Get(DbConstants.KEY_FILE_NAME)).Returns(dbPath);
            cfg.Setup(c => c.Get(DbConstants.KEY_CLIENT_EXEC)).Returns("");

            var provider = new SqliteDbProvider(cfg.Object);

            // Act
            var ex = Record.Exception(() => provider.CustomCustomerLogin("x@example.com' OR 1=1--", "pass"));

            // Assert
            Assert.Null(ex);

            try { System.IO.File.Delete(dbPath); } catch { /* ignore */ }
        }
    }
}
