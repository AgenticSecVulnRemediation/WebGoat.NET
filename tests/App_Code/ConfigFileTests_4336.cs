using System;
using System.IO;
using OWASP.WebGoat.NET.App_Code;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class ConfigFileTests
    {
        [Fact]
        public void Load_WithRootedPath_ThrowsArgumentException()
        {
            // Arrange
            var rooted = Path.Combine(Path.GetPathRoot(Environment.CurrentDirectory)!, "webgoat.config");
            var cfg = new ConfigFile(rooted);

            // Act + Assert
            Assert.Throws<ArgumentException>(() => cfg.Load());
        }

        [Fact]
        public void Load_WithTraversalPath_ThrowsArgumentException()
        {
            // Arrange
            var cfg = new ConfigFile(".." + Path.DirectorySeparatorChar + "webgoat.config");

            // Act + Assert
            Assert.Throws<ArgumentException>(() => cfg.Load());
        }
    }
}
