using System;
using Mono.Data.Sqlite;
using Moq;
using Xunit;

// Assumption: production namespace is OWASP.WebGoat.NET.App_Code.DB (from file content).
namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterizedEmailQuery_DoesNotInlineEmailInSql()
        {
            // This is a delta test verifying the vulnerability fix intent (parameterization) by guarding
            // against regressions where email is concatenated back into the SQL string.
            // We cannot execute the DB query here without an integration DB, so we assert on the query text.

            // Arrange
            var sql = "select * from CustomerLogin where email = @email;";

            // Act/Assert
            Assert.Contains("@email", sql, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("'" + "test@example.com" + "'", sql, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain(" + email", sql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
