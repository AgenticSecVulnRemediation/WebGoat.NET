using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class StoredXSSTests
    {
        [Fact]
        public void StoredXssPage_EnablesValidateRequest()
        {
            // Arrange
            // This delta test verifies the Page directive was hardened to validateRequest="true".
            var content = File.ReadAllText("WebGoat/Content/StoredXSS.aspx");

            // Act
            var match = Regex.Match(content, "<%@\\s*Page[^%]*%>", RegexOptions.IgnoreCase);

            // Assert
            Assert.True(match.Success);
            Assert.Contains("validateRequest=\"true\"", match.Value, System.StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("validateRequest=\"false\"", match.Value, System.StringComparison.OrdinalIgnoreCase);
        }
    }
}
