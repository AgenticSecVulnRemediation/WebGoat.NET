using System.IO;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class StoredXssDeltaTests
    {
        [Fact]
        public void StoredXssAspx_ShouldEnableValidateRequest()
        {
            // Arrange
            // This is a simple regression test for the delta change in StoredXSS.aspx:
            // validateRequest was switched from false to true.
            var path = Path.Combine("WebGoat", "Content", "StoredXSS.aspx");

            // Act
            var content = File.ReadAllText(path);

            // Assert
            Assert.Contains("validateRequest=\"true\"", content);
            Assert.DoesNotContain("validateRequest=\"false\"", content);
        }
    }
}
