using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class StoredXSSTests
    {
        [Fact]
        public void StoredXssMarkup_EnablesValidateRequest()
        {
            // Arrange
            // Regression: validateRequest must be true in StoredXSS.aspx page directive.
            var markup = File.ReadAllText(Path.Combine("WebGoat", "Content", "StoredXSS.aspx"));

            // Assert
            Assert.Matches(new Regex("validateRequest=\"true\"", RegexOptions.IgnoreCase), markup);
        }
    }
}
