using System;
using System.IO;
using Xunit;

namespace OWASP.WebGoat.NET.Content.Tests
{
    public class ReflectedXSSTests
    {
        [Fact]
        public void ReflectedXSSPageDirective_ValidateRequest_IsTrue()
        {
            // Arrange
            // This is a regression test over the security fix in the .aspx directive.
            // It asserts the page directive has validateRequest="true".
            var filePath = Path.Combine("WebGoat", "Content", "ReflectedXSS.aspx");

            // Act
            var content = File.ReadAllText(filePath);

            // Assert
            Assert.Contains("validateRequest=\"true\"", content);
            Assert.DoesNotContain("validateRequest=\"false\"", content);
        }
    }
}
