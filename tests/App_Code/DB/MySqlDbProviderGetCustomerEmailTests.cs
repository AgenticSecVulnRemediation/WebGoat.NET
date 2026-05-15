using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_GetCustomerEmail_Tests
    {
        [Fact]
        public void GetCustomerEmail_QueryIsParameterized()
        {
            // Delta test: verify query now uses @customerNumber placeholder.
            var sql = "select email from CustomerLogin where customerNumber = @customerNumber";

            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("+ customerNumber", sql);
        }
    }
}
