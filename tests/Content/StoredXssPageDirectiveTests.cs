using Xunit;
using System.IO;
using System.Text.RegularExpressions;

namespace OWASP.WebGoat.NET.Tests
{
    public class StoredXssPageDirectiveTests
    {
        [Fact]
        public void StoredXSS_PageDirective_EnablesRequestValidation()
        {
            // This tests the delta behavior: validateRequest was changed from false to true.
            // We validate by parsing the .aspx content provided in the PR.

            // NOTE: This test reads the file from the repo working directory.
            // If test runner uses different base path, adjust accordingly in build config.
            var path = Path.Combine("WebGoat", "Content", "StoredXSS.aspx");
            var content = File.ReadAllText(path);

            Assert.Matches(new Regex("validateRequest=\"true\"", RegexOptions.IgnoreCase), content);
            Assert.DoesNotMatch(new Regex("validateRequest=\"false\"", RegexOptions.IgnoreCase), content);
        }
    }
}
