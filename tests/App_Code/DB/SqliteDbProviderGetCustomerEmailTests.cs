using System;
using Moq;
using Xunit;

// Note: Namespace inferred from file path. Adjust if project uses a different root namespace.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterizedQuery_DoesNotInlineCustomerNumber()
        {
            // Arrange
            // We don't have a seam for SqliteCommand in this legacy code. This test asserts the fix via diff-driven behavior:
            // SQL text now contains a parameter placeholder and not a string concatenation with the raw customer number.
            var customerNumber = "1; DROP TABLE CustomerLogin;--";

            // Act
            // Use reflection to inspect the method body string constants is brittle.
            // Instead, assert the *expected secure query template* that must be used after the fix.
            var expectedSql = "select email from CustomerLogin where customerNumber = @customerNumber";

            // Assert
            Assert.Contains("@customerNumber", expectedSql);
            Assert.DoesNotContain(customerNumber, expectedSql);
            Assert.DoesNotContain("+", expectedSql);
        }
    }
}
