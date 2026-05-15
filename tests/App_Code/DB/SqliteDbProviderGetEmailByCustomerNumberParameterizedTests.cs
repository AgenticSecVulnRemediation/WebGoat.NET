using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetEmailByCustomerNumberParameterizedTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameter_NotConcatenation()
        {
            const string expected = "select email from CustomerLogin where customerNumber = @num";
            Assert.Contains("@num", expected);
            Assert.DoesNotContain("+ num", expected);
        }
    }
}
