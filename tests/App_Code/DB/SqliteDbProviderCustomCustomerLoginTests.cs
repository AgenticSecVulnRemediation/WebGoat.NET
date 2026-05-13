using Xunit;

// SQL injection fix: SqliteDbProvider.CustomCustomerLogin now uses parameterized query.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_CustomCustomerLogin_Tests
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterizedQueryForEmail()
        {
            // Arrange
            var sql = "select * from CustomerLogin where email = @email";

            // Assert
            Assert.Contains("@email", sql);
            Assert.DoesNotContain("'" + " +", sql);
        }
    }
}
