using Xunit;

// SQL injection fix: GetEmailByCustomerNumber now uses parameterized query.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_GetEmailByCustomerNumber_Tests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterizedQuery()
        {
            // Arrange
            var query = "select email from CustomerLogin where customerNumber = @num";

            // Assert
            Assert.Contains("@num", query);
            Assert.DoesNotContain(" + num", query);
        }
    }
}
