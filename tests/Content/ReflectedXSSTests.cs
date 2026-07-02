using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class ReflectedXSSTests
    {
        [Fact]
        public void ReflectedXssAspx_PageDirective_HasValidateRequestEnabled()
        {
            // Arrange
            // This delta test validates that validateRequest has been turned on in the page directive.
            // We read the .aspx content as text and assert the directive attribute.
            var aspxPath = Path.Combine("WebGoat", "Content", "ReflectedXSS.aspx");

            // Act
            var content = File.ReadAllText(aspxPath);

            // Assert
            Assert.Matches(new Regex("validateRequest=\"true\"", RegexOptions.IgnoreCase), content);
            Assert.DoesNotMatch(new Regex("validateRequest=\"false\"", RegexOptions.IgnoreCase), content);
        }
    }
}
