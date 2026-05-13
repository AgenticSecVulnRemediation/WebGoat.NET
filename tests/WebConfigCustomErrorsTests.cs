using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfigCustomErrorsTests
    {
        [Fact]
        public void WebConfig_CustomErrors_RemoteOnly()
        {
            // Arrange
            var config = File.ReadAllText(Path.Combine("WebGoat", "Web.config"));

            // Assert
            Assert.Matches(new Regex("<customErrors[^>]*mode=\"RemoteOnly\"", RegexOptions.IgnoreCase), config);
        }
    }
}
