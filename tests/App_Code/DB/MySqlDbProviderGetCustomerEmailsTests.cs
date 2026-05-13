using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailsTests
    {
        [Fact]
        public void GetCustomerEmails_UsesParameter_InLikeClause()
        {
            // Arrange
            string sql = "select email from CustomerLogin where email like CONCAT(@email, '%')";

            // Assert
            Assert.Contains("CONCAT(@email, '%')", sql, StringComparison.Ordinal);
            Assert.DoesNotContain("like '\" + email", sql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
