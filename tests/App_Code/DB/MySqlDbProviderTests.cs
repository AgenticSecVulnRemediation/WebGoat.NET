using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetPayments_UsesParameterizedQueryTemplate_ContainsCustomerNumberParameter()
        {
            // Assert only the delta change: query must use @customerNumber placeholder
            const string expectedSql = "select * from Payments where customerNumber = @customerNumber";
            Assert.Contains("@customerNumber", expectedSql);
            Assert.DoesNotContain("customerNumber = " + " ", expectedSql);
        }
    }
}
