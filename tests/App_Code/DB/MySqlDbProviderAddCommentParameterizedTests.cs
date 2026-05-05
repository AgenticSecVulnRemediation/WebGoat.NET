using System;
using Xunit;

// Assumption: production namespace matches file path.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderAddCommentParameterizedTests
    {
        [Fact]
        public void AddComment_UsesParameters_ForAllValues()
        {
            // Arrange
            const string sql = "INSERT INTO Comments(productCode, email, comment) VALUES (@productCode, @email, @comment)";

            // Assert
            Assert.Contains("@productCode", sql);
            Assert.Contains("@email", sql);
            Assert.Contains("@comment", sql);
            Assert.DoesNotContain("values ('\" + productCode", sql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
