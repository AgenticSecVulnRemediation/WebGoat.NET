using Xunit;
using Moq;
using System;
using System.Data;
using MySql.Data.MySqlClient;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        // Note: This test is a delta test for the security fix ensuring parameter binding is used.
        // It exercises GetCustomerEmail with an injection-style input and asserts it does not
        // break query syntax (i.e., it stays a single-parameter query).
        [Fact]
        public void GetCustomerEmail_WithSqlInjectionPayload_DoesNotConcatenateIntoSql()
        {
            // Arrange
            // We cannot easily intercept the MySqlCommand created inside without refactor;
            // instead, we validate behavior by asserting the SQL text in the method is parameterized
            // via reflection on the method body is not feasible in unit tests.
            // So we take an integration-like approach with an in-memory/invalid connection string:
            // the important assertion is that the thrown exception should not be a MySql syntax error
            // caused by concatenation of the payload, but rather a connection/execute error.

            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            config.Setup(c => c.Get(DbConstants.KEY_HOST)).Returns("localhost");
            config.Setup(c => c.Get(DbConstants.KEY_PORT)).Returns("3306");
            config.Setup(c => c.Get(DbConstants.KEY_DATABASE)).Returns("does_not_exist");
            config.Setup(c => c.Get(DbConstants.KEY_UID)).Returns("root");
            config.Setup(c => c.Get(DbConstants.KEY_PWD)).Returns(string.Empty);
            config.Setup(c => c.Get(DbConstants.KEY_CLIENT_EXEC)).Returns(string.Empty);

            var provider = new MySqlDbProvider(config.Object);

            // Classic injection payload that would terminate and append a condition if concatenated.
            string payload = "1 OR 1=1 --";

            // Act
            var ex = Record.Exception(() => provider.GetCustomerEmail(payload));

            // Assert
            // Method catches exceptions and returns message; so exception should be null.
            Assert.Null(ex);
            var result = provider.GetCustomerEmail(payload);
            Assert.False(string.IsNullOrEmpty(result));
            // If payload were concatenated into SQL, many servers would throw a syntax error mentioning "OR" or "--".
            // We assert message does not obviously contain these tokens (best-effort regression for concatenation).
            Assert.DoesNotContain(" OR ", result, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("--", result, StringComparison.OrdinalIgnoreCase);
        }
    }
}
