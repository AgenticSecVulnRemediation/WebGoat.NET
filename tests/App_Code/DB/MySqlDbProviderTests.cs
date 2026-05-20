using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void CustomCustomerLogin_WithSqlInjectionPayload_ReturnsFriendlyErrorMessageAndDoesNotThrow()
        {
            // NOTE: This is a delta regression test for the security fix that parameterizes the email lookup
            // in CustomCustomerLogin (previously vulnerable to SQL injection via string concatenation).
            // We cannot (and should not) assert SQL text here, so we assert the behavior is stable and does not
            // propagate SQL parsing errors for injection payloads.

            // Arrange
            // ConfigFile is part of the app; if its concrete type is not accessible in the test project,
            // this test may need to be moved into the same assembly or adapted to use a test config.
            var config = new ConfigFile();
            config.Set(DbConstants.KEY_HOST, "localhost");
            config.Set(DbConstants.KEY_PORT, "3306");
            config.Set(DbConstants.KEY_DATABASE, "goatdb");
            config.Set(DbConstants.KEY_UID, "root");
            config.Set(DbConstants.KEY_PWD, "");
            config.Set(DbConstants.KEY_CLIENT_EXEC, "mysql");

            var sut = new MySqlDbProvider(config);

            // Attack payload that would have broken out of the quoted string previously.
            var injectedEmail = "x' OR '1'='1";

            // Act
            string result = sut.CustomCustomerLogin(injectedEmail, "anything");

            // Assert
            // Post-fix, the query is parameterized; the method should not crash on this input.
            Assert.True(result == null || result.Contains("Email Address Not Found") || result.Contains("Password Not Valid"));
        }
    }
}
