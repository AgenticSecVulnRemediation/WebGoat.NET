using System;
using Xunit;
using Moq;

using OWASP.WebGoat.NET.App_Code;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class ConfigFilePathTraversalTests
    {
        [Theory]
        [InlineData("../secret.config")]
        [InlineData("..\\secret.config")]
        public void Load_WithParentTraversal_Throws(string fileName)
        {
            // Arrange
            var cfg = new ConfigFile(fileName);

            // Act + Assert
            Assert.Throws<Exception>(() => cfg.Load());
        }

        [Fact]
        public void Load_WithRootedPath_Throws()
        {
            // Arrange
            var rooted = Environment.OSVersion.Platform == PlatformID.Win32NT
                ? "C:\\Windows\\win.ini"
                : "/etc/passwd";

            var cfg = new ConfigFile(rooted);

            // Act + Assert
            Assert.Throws<Exception>(() => cfg.Load());
        }
    }
}
