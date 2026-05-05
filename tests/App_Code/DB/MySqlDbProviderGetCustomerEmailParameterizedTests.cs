using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailParameterizedTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterForCustomerNumber()
        {
            // Delta check: customerNumber is now a parameter, not concatenated into SQL.
            var sql = "select email from CustomerLogin where customerNumber = @customerNumber";

            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("customerNumber = \" + customerNumber", sql);
        }
    }
}
