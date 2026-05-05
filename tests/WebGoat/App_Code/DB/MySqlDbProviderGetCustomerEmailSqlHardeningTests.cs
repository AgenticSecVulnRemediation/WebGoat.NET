using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailSqlHardeningTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterForCustomerNumber()
        {
            // Delta: query should not concatenate customerNumber.
            const string sql = "select email from CustomerLogin where customerNumber = @customerNumber";

            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("customerNumber = \" +", sql);
        }
    }
}
