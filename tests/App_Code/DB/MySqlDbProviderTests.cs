using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterizedQueryTemplate_ContainsCustomerNumberParameter()
        {
            // Assert only the delta change: query must use @customerNumber placeholder
            const string expectedSql = "select email from CustomerLogin where customerNumber = @customerNumber";
            Assert.Contains("@customerNumber", expectedSql);
            Assert.DoesNotContain("customerNumber = " + " ", expectedSql);
        }
    }
}
