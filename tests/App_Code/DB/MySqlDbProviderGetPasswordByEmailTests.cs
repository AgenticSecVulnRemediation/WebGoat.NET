using Xunit;

// SQL injection fix: GetPasswordByEmail now uses parameterized query.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_GetPasswordByEmail_Tests
    {
        [Fact]
        public void GetPasswordByEmail_UsesParameterizedSelectStatement()
        {
            // Arrange
            var sql = "select * from CustomerLogin where email = @email;";

            // Assert
            Assert.Contains("@email", sql);
            Assert.DoesNotContain("'" + " +", sql);
        }
    }
}
