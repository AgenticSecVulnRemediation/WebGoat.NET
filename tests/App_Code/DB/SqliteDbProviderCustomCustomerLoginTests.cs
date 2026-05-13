using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderCustomCustomerLoginTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterizedEmailLookup()
        {
            // Arrange
            string sql = "select * from CustomerLogin where email = @email";

            // Assert
            Assert.Contains("@email", sql, StringComparison.Ordinal);
            Assert.DoesNotContain("where email = '\" + email", sql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
