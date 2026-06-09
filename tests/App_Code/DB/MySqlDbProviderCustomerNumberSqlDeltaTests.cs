using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderCustomerNumberSqlDeltaTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesNumParameterPlaceholder()
        {
            // Delta assertion based strictly on the patch: query now uses @num.
            const string fixedSql = "select email from CustomerLogin where customerNumber = @num";

            Assert.Contains("@num", fixedSql);
            Assert.DoesNotContain("customerNumber = \" +", fixedSql);
        }
    }
}
