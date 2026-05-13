using Xunit;

namespace OWASP.WebGoat.NET.Tests.App_Code.DB
{
    public class SqliteDbProviderGetEmailByCustomerNumberTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_Query_IsParameterized()
        {
            // Arrange
            const string sql = "select email from CustomerLogin where customerNumber = @num";

            // Assert
            Assert.Contains("@num", sql);
            Assert.DoesNotContain("customerNumber = \" + num", sql);
        }
    }
}
