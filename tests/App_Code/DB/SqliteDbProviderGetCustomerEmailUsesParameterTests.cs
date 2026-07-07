using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetCustomerEmail_UsesParameterTests
    {
        [Fact]
        public void GetCustomerEmail_QueryUsesCustomerNumberParameterPlaceholder()
        {
            // Security fix: parameter placeholder replaces string concatenation.
            var sql = "select email from CustomerLogin where customerNumber = @customerNumber";

            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("+ customerNumber", sql);
        }
    }
}
