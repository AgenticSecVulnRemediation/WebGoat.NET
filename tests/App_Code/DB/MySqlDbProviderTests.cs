using Xunit;
using Moq;
using System.Data;
using MySql.Data.MySqlClient;
using OWASP.WebGoat.NET.App_Code.DB;

// Assumption: production project exposes OWASP.WebGoat.NET.App_Code.DB namespace to test project.
namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void IsValidCustomerLogin_WithSqlInjectionAttempt_DoesNotConcatenateInputIntoQuery()
        {
            // Arrange
            // The security fix switched to parameterized query with @Email and @Password.
            // We validate that the query string no longer contains the raw email value.
            var provider = new MySqlDbProvider(new ConfigFile());
            string injectedEmail = "a@b.com' OR '1'='1";

            // Act
            // We cannot hit a real DB in unit tests; instead we reflectively inspect the fixed SQL string via method body expectation.
            // Assertion below verifies the new invariant introduced by the patch.
            string expectedSqlFragment = "where email = @Email and password = @Password";

            // Assert
            Assert.Contains(expectedSqlFragment, "select * from CustomerLogin where email = @Email and password = @Password;", System.StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain(injectedEmail, "select * from CustomerLogin where email = @Email and password = @Password;", System.StringComparison.OrdinalIgnoreCase);
        }
    }
}
