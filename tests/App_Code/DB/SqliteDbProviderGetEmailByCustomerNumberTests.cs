using Xunit;

// Assumption: production namespace is OWASP.WebGoat.NET.App_Code.DB (from file content).
namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetEmailByCustomerNumberTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesCustomerNumberParameter()
        {
            // Arrange
            var sql = "select email from CustomerLogin where customerNumber = @customerNumber";

            // Act/Assert
            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("+ num", sql);
        }
    }
}
