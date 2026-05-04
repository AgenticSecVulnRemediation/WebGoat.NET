using System;
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
            var method = typeof(MySqlDbProvider).GetMethod("GetCustomerEmails");
            Assert.NotNull(method);

            // Act
            const string expectedSql = "select email from CustomerLogin where email like @email";
            string expectedParamValue = "test" + "%";

            // Assert
            Assert.Contains("like @email", expectedSql, StringComparison.OrdinalIgnoreCase);
            Assert.EndsWith("%", expectedParamValue, StringComparison.Ordinal);
            Assert.DoesNotContain("like '", expectedSql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
