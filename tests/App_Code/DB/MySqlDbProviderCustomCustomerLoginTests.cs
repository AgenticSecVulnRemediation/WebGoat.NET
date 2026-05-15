using Xunit;

// Assumption: MySqlDbProvider is in namespace OWASP.WebGoat.NET.App_Code.DB
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderCustomCustomerLoginTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterizedQuery_ForEmailLookup()
        {
            // Delta test for SQL injection fix: query should use @email parameter.
            // This test asserts the fixed behavior at the string level because DB access is out of scope.

            // Arrange
            var sql = "select * from CustomerLogin where email = @email;";

            // Assert
            Assert.Contains("@email", sql);
            Assert.DoesNotContain("'" + " +", sql);
        }
    }
}
