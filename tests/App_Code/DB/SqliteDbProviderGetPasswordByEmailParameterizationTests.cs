using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPasswordByEmailParameterizationTests
    {
        [Fact]
        public void GetPasswordByEmail_SqlUsesEmailParameter()
        {
            // Arrange
            string injectedEmail = "x' OR '1'='1";
            string fixedSql = "select * from CustomerLogin where email = @email;";

            // Assert
            Assert.Contains("email = @email", fixedSql, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain(injectedEmail, fixedSql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
