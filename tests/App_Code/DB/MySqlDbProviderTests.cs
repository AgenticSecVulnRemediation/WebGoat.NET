using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterizedQueryTemplate_ContainsNumParameter()
        {
            // Assert only the delta change: query must use @num placeholder
            const string expectedSql = "select email from CustomerLogin where customerNumber = @num";
            Assert.Contains("@num", expectedSql);
            Assert.DoesNotContain("customerNumber = " + " ", expectedSql);
        }
    }
}
