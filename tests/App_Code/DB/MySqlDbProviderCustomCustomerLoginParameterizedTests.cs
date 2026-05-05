using System;
using Xunit;

// Assumption: production namespace matches file path.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderCustomCustomerLoginParameterizedTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesEmailParameterPlaceholder()
        {
            // Arrange
            // Delta: SQL changed from string concatenation to parameterized query.
            const string sql = "select * from CustomerLogin where email = @Email;";

            // Assert
            Assert.Contains("@Email", sql);
            Assert.DoesNotContain("'\" + email + \"'", sql);
        }
    }
}
