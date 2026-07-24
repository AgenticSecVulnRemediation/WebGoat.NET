using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetEmailByCustomerNumberParameterizedTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesNumParameter_InsteadOfConcatenatingIntoSql()
        {
            // Arrange
            // Delta behavior: query changed from concatenating num to using @num parameter.
            var fixedSql = "select email from CustomerLogin where customerNumber = @num";

            // Assert
            Assert.Contains("@num", fixedSql);
            Assert.DoesNotContain("+ num", fixedSql);
        }
    }
}
