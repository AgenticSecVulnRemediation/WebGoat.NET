using System.IO;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class ReflectedXss_PageDirectiveTests
    {
        [Fact]
        public void PageDirective_ShouldEnableRequestValidation()
        {
            // Arrange
            // This is a delta regression test: validateRequest changed from false -> true.
            var content = File.ReadAllText("WebGoat/Content/ReflectedXSS.aspx");

            // Assert
            Assert.Contains("validateRequest=\"true\"", content);
            Assert.DoesNotContain("validateRequest=\"false\"", content);
        }
    }
}
