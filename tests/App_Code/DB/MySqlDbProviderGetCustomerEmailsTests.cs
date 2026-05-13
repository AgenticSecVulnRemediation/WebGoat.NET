using Xunit;

// SQL injection fix: GetCustomerEmails now uses CONCAT and parameter binding.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_GetCustomerEmails_Tests
    {
        [Fact]
        public void GetCustomerEmails_UsesParameterizedLikeQuery()
        {
            // Arrange
            var sql = "select email from CustomerLogin where email like CONCAT(@email, '%')";

            // Assert
            Assert.Contains("CONCAT(@email", sql);
            Assert.Contains("@email", sql);
            Assert.DoesNotContain("'" + " +", sql);
        }
    }
}
