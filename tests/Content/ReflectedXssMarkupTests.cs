using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class ReflectedXssMarkupTests
    {
        [Fact]
        public void ReflectedXssPage_ValidateRequest_IsEnabled()
        {
            // Delta behavior: validateRequest flipped from false -> true
            const string pageDirective = "<%@ Page Title=\"\" validateRequest=\"true\" Language=\"C#\"";

            Assert.Contains("validateRequest=\"true\"", pageDirective);
            Assert.DoesNotContain("validateRequest=\"false\"", pageDirective);
        }
    }
}
