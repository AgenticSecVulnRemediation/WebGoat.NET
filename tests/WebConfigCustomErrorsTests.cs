using System;
using System.IO;
using Xunit;

// Assumptions:
// - No XML configuration loader is exposed for unit tests.
// - We validate the delta by checking customErrors is no longer "Off" and that the expected
//   RemoteOnly configuration exists in the file content.

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfigCustomErrorsTests
    {
        [Fact]
        public void WebConfig_CustomErrors_IsRemoteOnly_WithDefaultRedirect()
        {
            // Arrange
            var baseDir = AppContext.BaseDirectory;
            string? webConfigPath = FindFileUpwards(baseDir, Path.Combine("WebGoat", "Web.config"));
            Assert.True(File.Exists(webConfigPath), $"Could not locate WebGoat/Web.config from base directory: {baseDir}");

            var xml = File.ReadAllText(webConfigPath!);

            // Act / Assert
            Assert.DoesNotContain("<customErrors mode=\"Off\"", xml, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("<customErrors mode=\"RemoteOnly\"", xml, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("defaultRedirect=\"ErrorPage.aspx\"", xml, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("<error statusCode=\"404\" redirect=\"NotFound.aspx\"", xml, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("<error statusCode=\"500\" redirect=\"ErrorPage.aspx\"", xml, StringComparison.OrdinalIgnoreCase);
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
