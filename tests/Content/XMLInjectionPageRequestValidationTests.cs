using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace OWASP.WebGoat.NET.Content.Tests
{
    public class XMLInjectionPageRequestValidationTests
    {
        [Fact]
        public void XmlInjectionPage_HasValidateRequestEnabled()
        {
            // Arrange
            var path = Path.Combine("WebGoat", "Content", "XMLInjection.aspx");

            // Act
            var content = File.ReadAllText(path);

            // Assert
            // Delta test: validateRequest changed false -> true.
            Assert.Matches(new Regex("validateRequest=\"true\"", RegexOptions.IgnoreCase), content);
            Assert.DoesNotMatch(new Regex("validateRequest=\"false\"", RegexOptions.IgnoreCase), content);
        }
    }
}
