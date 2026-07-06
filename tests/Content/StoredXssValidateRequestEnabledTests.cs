using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class StoredXssValidateRequestEnabledTests
    {
        [Fact]
        public void StoredXssPage_ValidateRequest_IsEnabled()
        {
            // Delta regression: StoredXSS.aspx changed validateRequest="false" to "true".
            var src = System.IO.File.ReadAllText("WebGoat/Content/StoredXSS.aspx");

            Assert.Contains("validateRequest=\"true\"", src);
            Assert.DoesNotContain("validateRequest=\"false\"", src);
        }
    }
}
