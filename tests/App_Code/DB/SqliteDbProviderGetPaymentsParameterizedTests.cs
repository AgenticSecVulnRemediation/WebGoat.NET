using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPaymentsParameterizedTests
    {
        [Fact]
        public void GetPayments_SqlText_UsesCustomerNumberParameter()
        {
            // Delta regression: PR changed SQL from concatenation to parameter binding.
            const string sql = "select * from Payments where customerNumber = @customerNumber";

            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("+", sql);
        }
    }
}
