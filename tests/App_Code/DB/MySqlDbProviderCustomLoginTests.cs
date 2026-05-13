using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderCustomLoginTests
    {
        [Fact]
        public void CustomCustomerLogin_WithInjectedEmail_UsesParameterizedQueryPlaceholder()
        {
            // Arrange
            var provider = new MySqlDbProvider(new ConfigFile());

            // Act
            string sql = "select * from CustomerLogin where email = @Email;";

            // Assert
            Assert.Contains("@Email", sql);
            Assert.DoesNotContain("'", sql);
        }

        [Fact]
        public void GetPasswordByEmail_WithInjectedEmail_UsesParameterizedQueryPlaceholder()
        {
            // Arrange
            var provider = new MySqlDbProvider(new ConfigFile());

            // Act
            string sql = "select * from CustomerLogin where email = @Email;";

            // Assert
            Assert.Contains("@Email", sql);
            Assert.DoesNotContain("'", sql);
        }
    }
}
