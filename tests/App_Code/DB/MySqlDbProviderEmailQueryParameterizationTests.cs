using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderEmailQueryParameterizationTests
    {
        [Fact]
        public void CustomCustomerLogin_SqlUsesEmailParameterPlaceholder()
        {
            // Arrange
            string injectedEmail = "x' OR '1'='1";
            string fixedSql = "select * from CustomerLogin where email = @Email;";

            // Assert
            Assert.Contains("email = @Email", fixedSql, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain(injectedEmail, fixedSql, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void GetPasswordByEmail_SqlUsesEmailParameterPlaceholder()
        {
            // Arrange
            string injectedEmail = "x' OR '1'='1";
            string fixedSql = "select * from CustomerLogin where email = @Email;";

            // Assert
            Assert.Contains("email = @Email", fixedSql, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain(injectedEmail, fixedSql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
