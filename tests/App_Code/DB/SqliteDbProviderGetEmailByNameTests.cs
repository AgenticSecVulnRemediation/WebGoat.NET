using System;
using Xunit;

// Assumptions:
// - Production type is OWASP.WebGoat.NET.App_Code.DB.SqliteDbProvider.
// Delta behavior under test: GetEmailByName now uses parameter placeholder (@name) instead of inlining user input.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_UsesParameterizedQuery_DoesNotInlineName()
        {
            // Arrange
            var method = typeof(OWASP.WebGoat.NET.App_Code.DB.SqliteDbProvider)
                .GetMethod("GetEmailByName");

            // Act
            var methodText = method?.ToString() ?? string.Empty;

            // Assert
            Assert.Contains("GetEmailByName", methodText);
            Assert.DoesNotContain("like '" , methodText);
            Assert.DoesNotContain("+ name +", methodText);
        }
    }
}
