using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests.Content
{
    public class StoredXss_PageDirectiveRequestValidationTests
    {
        [Fact]
        public void StoredXss_PageDirective_HasValidateRequestEnabled()
        {
            // Delta guard for PR #423: validateRequest changed from false to true.
            var source = LoadMarkup();

            Assert.Contains("validateRequest=\"true\"", source);
            Assert.DoesNotContain("validateRequest=\"false\"", source);
        }

        private static string LoadMarkup()
        {
            var path = System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "Content", "StoredXSS.aspx");
            path = System.IO.Path.GetFullPath(path);
            return System.IO.File.ReadAllText(path);
        }
    }
}
