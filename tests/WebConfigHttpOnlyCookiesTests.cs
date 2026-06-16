using System;
using System.IO;
using System.Reflection;
using Xunit;

// Assumptions:
// - No dedicated config parser is available in the test project.
// - We validate the security-relevant delta by checking the Web.config XML contains the
//   exact attribute value set by this patch.

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfigHttpOnlyCookiesTests
    {
        [Fact]
        public void WebConfig_HttpCookiesHttpOnlyCookies_IsTrue()
        {
            // Arrange
            var baseDir = AppContext.BaseDirectory;

            // Attempt to locate WebGoat/Web.config relative to test output.
            // This is intentionally robust for CI layouts.
            string? webConfigPath = FindFileUpwards(baseDir, Path.Combine("WebGoat", "Web.config"));
            Assert.True(File.Exists(webConfigPath), $"Could not locate WebGoat/Web.config from base directory: {baseDir}");

            var xml = File.ReadAllText(webConfigPath!);

            // Act / Assert
            Assert.Contains("<httpCookies httpOnlyCookies=\"true\"", xml, StringComparison.OrdinalIgnoreCase);
        }

        private static string? FindFileUpwards(string startDir, string relativePath)
        {
            var dir = new DirectoryInfo(startDir);
            while (dir != null)
            {
                var candidate = Path.Combine(dir.FullName, relativePath);
                if (File.Exists(candidate))
                {
                    return candidate;
                }
                dir = dir.Parent;
            }
            return null;
        }
    }
}
