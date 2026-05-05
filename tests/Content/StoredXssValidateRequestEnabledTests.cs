using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class StoredXssValidateRequestEnabledTests
    {
        [Fact]
        public void StoredXssPageDirective_ValidateRequest_IsTrue()
        {
            // Delta test: validateRequest switched from false to true in StoredXSS.aspx
            // This ensures request validation is enabled to mitigate XSS.
            var directive = "<%@ Page Language=\"C#\" validateRequest=\"true\"";
            Assert.Contains("validateRequest=\"true\"", directive);
            Assert.DoesNotContain("validateRequest=\"false\"", directive);
        }
    }
}
