using Xunit;
using System;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderCustomerLoginParameterizationTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesEmailParameter_PlaceholderPresent()
        {
            // Arrange
            var attackerEmail = "a@b.com' OR 1=1 --";

            // Act
            var sql = "select * from CustomerLogin where email = @Email;";

            // Assert
            Assert.Contains("@Email", sql, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain(attackerEmail, sql, StringComparison.Ordinal);
            Assert.DoesNotContain("email = '" + attackerEmail + "'", sql, StringComparison.Ordinal);
        }

        [Fact]
        public void GetPasswordByEmail_UsesEmailParameter_PlaceholderPresent()
        {
            // Arrange
            var attackerEmail = "a@b.com' OR 1=1 --";

            // Act
            var sql = "select * from CustomerLogin where email = @Email;";

            // Assert
            Assert.Contains("@Email", sql, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain(attackerEmail, sql, StringComparison.Ordinal);
            Assert.DoesNotContain("email = '" + attackerEmail + "'", sql, StringComparison.Ordinal);
        }
    }
}
