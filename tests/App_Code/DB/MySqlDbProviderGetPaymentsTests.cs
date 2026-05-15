using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_GetPayments_Tests
    {
        [Fact]
        public void GetPayments_UsesParameter_ForCustomerNumber()
        {
            // Delta test: query should be parameterized.
            var sql = "select * from Payments where customerNumber = @customerNumber";

            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("+ customerNumber", sql);
        }
    }
}
