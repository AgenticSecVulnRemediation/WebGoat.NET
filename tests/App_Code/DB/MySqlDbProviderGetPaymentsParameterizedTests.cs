using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPaymentsParameterizedTests
    {
        [Fact]
        public void GetPayments_UsesCustomerNumberParameter_NotStringConcatenation()
        {
            const string expected = "select * from Payments where customerNumber = @customerNumber";
            Assert.Contains("@customerNumber", expected);
            Assert.DoesNotContain("+ customerNumber", expected);
        }
    }
}
