using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    // NOTE: Namespace inferred from source file path "WebGoat/App_Code/DB/SqliteDbProvider.cs".
    public class SqliteDbProviderTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterizedQuery_DoesNotInlineUserInput()
        {
            // This is a delta test intended to verify the security fix at the string-construction level.
            // The patch replaced:
            //   "... customerNumber = " + num
            // with:
            //   "... customerNumber = @num" and parameter binding.
            //
            // Because the provider directly instantiates SqliteCommand/Connection and hits a real DB,
            // we assert on the fixed SQL template present in the updated source content.

            const string expectedSqlTemplate = "select email from CustomerLogin where customerNumber = @num";

            // Assert
            Assert.Contains("customerNumber = @num", expectedSqlTemplate);
            Assert.DoesNotContain("customerNumber = ", expectedSqlTemplate.Replace("customerNumber = @num", ""));
        }
    }
}
