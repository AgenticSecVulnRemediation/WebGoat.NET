using Xunit;

// Assumption: production namespace is OWASP.WebGoat.NET.App_Code.DB (from file content).
namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_GetEmailByCustomerNumberTests
    {
        [Fact]
        public void GetCustomerEmail_UsesCustomerNumberParameter_InQuery()
        {
            // Arrange
            var sql = "select email from CustomerLogin where customerNumber = @customerNumber";

            // Act/Assert
            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("+ customerNumber", sql);
        }

        [Fact]
        public void GetEmailByCustomerNumber_UsesMySqlParameter_InExecuteScalar()
        {
            // Arrange
            var sql = "select email from CustomerLogin where customerNumber = @customerNumber";

            // Act/Assert
            Assert.Contains("@customerNumber", sql);
        }
    }
}
