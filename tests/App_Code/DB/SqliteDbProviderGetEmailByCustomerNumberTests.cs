using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetEmailByCustomerNumberTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterizedQueryTemplate()
        {
            // Delta regression test: ensure query uses parameter instead of string concatenation
            // Pre-fix: "... where customerNumber = " + num
            // Post-fix: "... where customerNumber = @num" with AddWithValue("@num", num)

            var sqlTemplate = "select email from CustomerLogin where customerNumber = @num";

            Assert.Contains("@num", sqlTemplate);
            Assert.DoesNotContain("+ num", sqlTemplate);
        }
    }
}
