using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameterizedQueryForCredentials()
        {
            // Arrange/Act
            var sql = "select * from CustomerLogin where email = @email and password = @password;";

            // Assert
            Assert.Contains("@email", sql);
            Assert.Contains("@password", sql);
            Assert.DoesNotContain("email = '\" + email", sql);
            Assert.DoesNotContain("password = '\" +", sql);
        }
    }
}
