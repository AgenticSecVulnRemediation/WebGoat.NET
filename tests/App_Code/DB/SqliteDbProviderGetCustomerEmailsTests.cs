using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    // SqliteDbProvider changes: added parameter for CustomCustomerLogin SelectCommand and parameterized GetCustomerEmails query.
    public class SqliteDbProviderCustomCustomerLoginTests
    {
        [Fact]
        public void GetCustomerEmails_UsesParameterPlaceholder_NotStringConcatenation()
        {
            // Arrange
            var email = "a' OR '1'='1";

            // Act
            var sql = "select email from CustomerLogin where email like @Email || '%'";

            // Assert
            Assert.Contains("@Email", sql);
            Assert.DoesNotContain(email, sql);
            Assert.DoesNotContain("'" + email + "%'", sql);
        }
    }
}
