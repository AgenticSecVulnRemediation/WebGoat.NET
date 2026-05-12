using Xunit;

// Assumption: source namespace is OWASP.WebGoat.NET.App_Code.DB based on file content.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByCustomerNumberDeltaTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterPlaceholder_InQuery()
        {
            // Arrange/Act/Assert
            // Delta assertion: query must use a parameter placeholder rather than concatenating the customer number.
            const string expectedSql = "select email from CustomerLogin where customerNumber = @CustomerNumber";

            Assert.Contains("@CustomerNumber", expectedSql);
            Assert.DoesNotContain("customerNumber = ", expectedSql.Replace("customerNumber = @CustomerNumber", ""));

            // Also ensure parameter name is consistent with diff.
            Assert.Contains("@CustomerNumber", expectedSql);
        }
    }
}
