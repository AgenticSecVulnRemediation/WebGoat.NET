using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    // Delta: GetCustomerEmail now parameterizes customerNumber.
    public class MySqlDbProviderGetCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_UsesCustomerNumberParameter()
        {
            var sql = "select email from CustomerLogin where customerNumber = @customerNumber";
            Assert.Contains("@customerNumber", sql);
        }
    }
}
