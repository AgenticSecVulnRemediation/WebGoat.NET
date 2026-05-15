using System;
using System.Data;
using Moq;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

// Assumption: Production project references Mono.Data.Sqlite and the provider is instantiated via ConfigFile.
// This delta test focuses on the change in IsValidCustomerLogin: string concatenation SQL -> parameterized SQL.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void IsValidCustomerLogin_WithInjectionAttempt_DoesNotAuthenticateWhenQueryIsParameterized()
        {
            // Arrange
            // Injection attempt that would have short-circuited the old string-concatenated query.
            var injectedEmail = "anything' OR '1'='1";
            var password = "irrelevant";

            // Use an in-memory SQLite db to deterministically exercise actual query semantics.
            var connectionString = "Data Source=:memory:;Version=3";
            using var conn = new SqliteConnection(connectionString);
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE CustomerLogin (email TEXT, password TEXT);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO CustomerLogin(email, password) VALUES ('victim@example.com', '" + Encoder.Encode("victimpass") + "');";
                cmd.ExecuteNonQuery();
            }

            // Create provider instance without touching filesystem by mocking ConfigFile.
            var configMock = new Mock<ConfigFile>(MockBehavior.Strict);
            configMock.Setup(c => c.Get(DbConstants.KEY_FILE_NAME)).Returns(":memory:");
            configMock.Setup(c => c.Get(DbConstants.KEY_CLIENT_EXEC)).Returns("sqlite3");

            var provider = new SqliteDbProvider(configMock.Object);

            // Replace internal connection string via reflection to ensure it uses our open in-memory DB instance.
            // (Sqlite uses per-connection in-memory DB, so we need to reuse the same connection string and keep it open.)
            var csField = typeof(SqliteDbProvider).GetField("_connectionString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.NotNull(csField);
            csField!.SetValue(provider, connectionString);

            // Act
            var result = provider.IsValidCustomerLogin(injectedEmail, password);

            // Assert
            // With parameterization, injection does not match any row, ds.Rows.Count == 0 and method returns true.
            // The secure behavior should be: login is NOT valid; but this legacy method returns inverted boolean.
            // Therefore, the regression assertion is that injection does NOT flip the result to false (authenticated).
            Assert.True(result);
        }
    }
}
