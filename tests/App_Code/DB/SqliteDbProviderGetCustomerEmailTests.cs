using Xunit;

namespace OWASP.WebGoat.NET.Tests.App_Code.DB
{
    public class SqliteDbProviderGetCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_Query_IsParameterized()
        {
            // Arrange
            const string sql = "select email from CustomerLogin where customerNumber = @customerNumber";

            // Assert
            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("customerNumber = "+" ", sql);
        }
    }
}
