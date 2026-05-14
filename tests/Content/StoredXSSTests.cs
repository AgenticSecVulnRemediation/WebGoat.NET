using System;
using System.IO;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class StoredXSSTests
    {
        [Fact]
        public void StoredXssMarkup_HasValidateRequestEnabled()
        {
            // Delta behavior: validateRequest set to true to enable ASP.NET request validation.
            // Read the markup file directly; this is deterministic within repo checkout.
            var path = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "WebGoat", "Content", "StoredXSS.aspx");
            var content = File.ReadAllText(path);
            Assert.Contains("validateRequest=\"true\"", content);
        }
    }
}
