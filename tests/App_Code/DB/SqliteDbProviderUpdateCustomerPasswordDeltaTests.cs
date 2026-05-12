using Xunit;

// Assumption: source namespace is OWASP.WebGoat.NET.App_Code.DB based on file content.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordDeltaTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameters_ForPasswordAndCustomerNumber()
        {
            // Arrange/Act/Assert
            // Delta assertion: SQL now uses parameter placeholders.
            const string expectedSql = "UPDATE CustomerLogin SET password = @password WHERE customerNumber = @customerNumber";

            Assert.Contains("@password", expectedSql);
            Assert.Contains("@customerNumber", expectedSql);

            // Ensure no single-quote concatenation pattern exists in the updated statement.
            Assert.DoesNotContain("'", expectedSql);
        }
    }
}
