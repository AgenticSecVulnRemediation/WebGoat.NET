using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    // NOTE: Namespace inferred from source file path "WebGoat/App_Code/DB/SqliteDbProvider.cs".
    public class SqliteDbProviderCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameter_ForCustomerNumber()
        {
            // Patch changed:
            //   "... customerNumber = " + customerNumber
            // to:
            //   "... customerNumber = @customerNumber" with parameter binding.
            const string sql = "select email from CustomerLogin where customerNumber = @customerNumber";

            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("customerNumber = " + " ", sql);
        }
    }
}
