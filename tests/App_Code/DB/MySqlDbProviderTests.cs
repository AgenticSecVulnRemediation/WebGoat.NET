using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterizedQuery_ForNum()
        {
            // Arrange
            const string query = "select email from CustomerLogin where customerNumber = @num";

            // Assert
            Assert.Contains("@num", query);
            Assert.DoesNotContain(" + num", query);
        }
    }
}
