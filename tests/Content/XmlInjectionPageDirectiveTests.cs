using Xunit;
using System.IO;

namespace OWASP.WebGoat.NET.Content.Tests
{
    public class XmlInjectionPageDirectiveTests
    {
        [Fact]
        public void XmlInjectionPageDirective_HasValidateRequestEnabled()
        {
            // Arrange
            var path = Path.Combine("WebGoat", "Content", "XMLInjection.aspx");

            // Act
            var content = File.ReadAllText(path);

            // Assert
            Assert.Contains("validateRequest=\"true\"", content);
            Assert.DoesNotContain("validateRequest=\"false\"", content);
        }
    }
}
