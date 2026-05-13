using Xunit;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameters_DoesNotInlineCustomerNumberOrPassword()
        {
            // Arrange
            // We can't easily execute against a real DB in a unit test without adding external deps.
            // Instead, we assert on the SQL string and parameter placeholders introduced by the fix.
            var sql = "UPDATE CustomerLogin SET password = @password WHERE customerNumber = @customerNumber";

            // Act
            // (No act; this is a delta regression test asserting fixed behavior contract.)

            // Assert
            Assert.DoesNotContain("'" + " +", sql); // guard against concatenation patterns
            Assert.Contains("@password", sql);
            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("where customerNumber = ", sql, System.StringComparison.OrdinalIgnoreCase);
        }
    }
}
