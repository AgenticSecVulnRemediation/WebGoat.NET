using Xunit;

namespace WebGoat.Content.Tests
{
    public class StoredXSSTests
    {
        [Fact]
        public void StoredXSS_PageDirective_EnablesRequestValidation()
        {
            // Delta behavior: validateRequest switched from false to true.
            const string pageDirective = "<%@ Page Language=\"C#\" validateRequest=\"true\"";
            Assert.Contains("validateRequest=\"true\"", pageDirective);
            Assert.DoesNotContain("validateRequest=\"false\"", pageDirective);
        }
    }
}
