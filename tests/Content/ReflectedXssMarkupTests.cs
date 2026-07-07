using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class ReflectedXssMarkupTests
    {
        [Fact]
        public void ReflectedXssPage_HasValidateRequestEnabled()
        {
            // Arrange
            // Delta test for PR #4014: validateRequest changed from false to true in ReflectedXSS.aspx markup.
            // Markup is not compiled into assembly; simplest deterministic check is to assert the file contains the attribute.

            var path = System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "Content", "ReflectedXSS.aspx");

            // Act
            var content = System.IO.File.ReadAllText(path);

            // Assert
            Assert.Contains("validateRequest=\"true\"", content);
            Assert.DoesNotContain("validateRequest=\"false\"", content);
        }
    }
}
