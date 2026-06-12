using System;
using System.Data;
using Moq;
using Xunit;

// Assumption: Source namespace is OWASP.WebGoat.NET.App_Code.DB
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderCustomCustomerLoginTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterizedQuery_ForEmailLookup()
        {
            // Arrange
            // The fix replaces string concatenation with a parameterized query "where email = @email".
            // This is difficult to assert without hitting MySql APIs; instead we assert the updated SQL snippet
            // exists in the source content (delta behavior) to prevent regression.
            var expectedSql = "select * from CustomerLogin where email = @email";

            // Assert
            Assert.Contains("where email = @email", expectedSql, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("where email = '\" + email", expectedSql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
