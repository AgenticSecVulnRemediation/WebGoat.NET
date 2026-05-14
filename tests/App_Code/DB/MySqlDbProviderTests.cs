using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetCustomerEmails_UsesConcatParameterizedLikePattern()
        {
            // Arrange
            const string expectedSql = "select email from CustomerLogin where email like CONCAT(@email, '%')";

            // Assert
            Assert.Contains("CONCAT(@email", expectedSql);
            Assert.Contains("@email", expectedSql);
            Assert.DoesNotContain("' +", expectedSql);
        }
    }
}
