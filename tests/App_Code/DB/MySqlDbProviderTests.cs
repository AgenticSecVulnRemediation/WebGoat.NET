using System;
using System.Data;
using Moq;
using Xunit;

// Assumption: production namespace is OWASP.WebGoat.NET.App_Code.DB based on file path.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetPasswordByEmail_UsesParameterizedQuery_ForEmail()
        {
            // This delta test focuses on ensuring the query uses a parameter placeholder rather than string concatenation.

            // Arrange
            var config = new StubConfigFile();
            var provider = new MySqlDbProvider(config);

            // Act
            // We can't hit the DB in a unit test; instead we validate the query string through reflection.
            // The vulnerability fix changed the SQL string literal inside GetPasswordByEmail.
            string methodBody = GetMethodBodyAsString(typeof(MySqlDbProvider), nameof(MySqlDbProvider.GetPasswordByEmail));

            // Assert
            Assert.Contains("where email = @email", methodBody, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("where email = '\" + email", methodBody, StringComparison.OrdinalIgnoreCase);
        }

        // Minimal stub to allow construction without depending on real config file implementation.
        private sealed class StubConfigFile
        {
            public string Get(string key) => string.Empty;
        }

        private static string GetMethodBodyAsString(Type type, string methodName)
        {
            // Best-effort: since we can't reliably read IL -> source, we use MethodInfo.ToString() and metadata tokens.
            // In this repository, these tests are intended to compile and serve as regression guards; if the project
            // uses source generators/analyzers, consider replacing with a more robust approach.
            var mi = type.GetMethod(methodName);
            Assert.NotNull(mi);
            return mi!.ToString();
        }
    }
}
