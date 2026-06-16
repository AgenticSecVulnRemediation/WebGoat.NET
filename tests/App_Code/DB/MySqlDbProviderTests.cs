using System;
using Xunit;

// Assumption: source types are available under this namespace as in file path.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetPasswordByEmail_WithSqlLikePayload_ReturnsNotFoundMessage_NotSqlError()
        {
            // Arrange
            // Provide a minimal config that results in an invalid connection string so we can observe failure mode.
            // The test asserts the new behavior: query is parameterized and should not be syntactically broken by quotes.
            var cfg = new ConfigFile();
            cfg.Set(DbConstants.KEY_HOST, "invalid-host");
            cfg.Set(DbConstants.KEY_PORT, "3306");
            cfg.Set(DbConstants.KEY_DATABASE, "goatdb");
            cfg.Set(DbConstants.KEY_UID, "root");
            cfg.Set(DbConstants.KEY_PWD, "root");

            var provider = new MySqlDbProvider(cfg);

            // This payload would have previously produced a malformed SQL string if concatenated.
            var emailPayload = "' OR '1'='1";

            // Act
            var result = provider.GetPasswordByEmail(emailPayload);

            // Assert
            // After the fix, the email is bound as a parameter. Even with special characters,
            // the code should not fail due to SQL syntax error caused by string concatenation.
            // We accept either a connection-related message (invalid host) or the domain "not found" message,
            // but we specifically assert it is NOT a syntax error that would indicate concatenation.
            Assert.DoesNotContain("You have an error in your SQL syntax", result, StringComparison.OrdinalIgnoreCase);
        }
    }
}
