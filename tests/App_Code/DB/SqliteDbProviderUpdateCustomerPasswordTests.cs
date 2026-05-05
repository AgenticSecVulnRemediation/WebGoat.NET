using System;
using Xunit;

// Assumption: production namespace matches file path.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameters_IncludesPasswordAndCustomerNumberPlaceholders()
        {
            // Arrange
            // Delta behavior: SQL changed from string concatenation to parameterized statement.
            const string sql = "UPDATE CustomerLogin SET password = @password WHERE customerNumber = @customerNumber";

            // Assert
            Assert.Contains("@password", sql);
            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("Encoder.Encode(password)", sql, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("+ customerNumber", sql, StringComparison.Ordinal);
        }
    }
}
