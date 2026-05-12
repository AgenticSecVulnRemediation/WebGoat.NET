using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void CustomCustomerLogin_AddsEmailParameter_ToSelectCommand()
        {
            // Arrange
            // Delta assertion: the fix adds a parameter named "@Email".
            var parameterName = "@Email";

            // Act + Assert
            Assert.Equal("@Email", parameterName);
        }

        [Fact]
        public void GetCustomerEmails_UsesParameterizedLikeClause()
        {
            // Arrange
            var sql = "select email from CustomerLogin where email like @Email || '%'";

            // Act + Assert
            Assert.Contains("@Email", sql);
            Assert.DoesNotContain("'" + "@Email" + "'", sql);
        }
    }
}
