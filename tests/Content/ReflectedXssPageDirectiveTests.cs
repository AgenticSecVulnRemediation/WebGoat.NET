using Xunit;
using System.IO;

namespace OWASP.WebGoat.NET.Content.Tests
{
    public class ReflectedXssPageDirectiveTests
    {
        [Fact]
        public void ReflectedXssPageDirective_HasValidateRequestEnabled()
        {
            // Arrange
            var path = Path.Combine("WebGoat", "Content", "ReflectedXSS.aspx");

            // Act
            var content = File.ReadAllText(path);

            // Assert
            Assert.Contains("validateRequest=\"true\"", content);
            Assert.DoesNotContain("validateRequest=\"false\"", content);
        }
    }
}
