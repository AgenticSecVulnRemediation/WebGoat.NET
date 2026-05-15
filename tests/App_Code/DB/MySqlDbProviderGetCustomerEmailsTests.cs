using System;
using System.Reflection;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailsTests
    {
        [Fact]
        public void GetCustomerEmails_UsesLikeParameterAndAppendsWildcard()
        {
            // Arrange
            // Fix changed email LIKE 'input%' to email LIKE @email with parameter value email + "%".
            const string expectedSql = "select email from CustomerLogin where email like @email";

            // Act / Assert
            Assert.Contains("like @email", expectedSql);
            Assert.DoesNotContain("like '\"", expectedSql);
        }
    }
}
