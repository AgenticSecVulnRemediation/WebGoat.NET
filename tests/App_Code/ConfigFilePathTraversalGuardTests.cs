using System;
using System.IO;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class ConfigFile_Load_PathTraversalGuardTests
    {
        [Theory]
        [InlineData("../config.txt")]
        [InlineData("..\\config.txt")]
        public void Load_WithTraversalSequence_ThrowsArgumentException(string path)
        {
            // Arrange
            var cfg = new OWASP.WebGoat.NET.App_Code.ConfigFile(path);

            // Act
            var ex = Assert.Throws<ArgumentException>(() => cfg.Load());

            // Assert
            Assert.Contains("Invalid file path", ex.Message);
        }

        [Fact]
        public void Load_WithRootedPath_ThrowsArgumentException()
        {
            // Arrange
            var rooted = Path.IsPathRooted("/tmp/config.txt") ? "/tmp/config.txt" : Path.GetFullPath("config.txt");
            var cfg = new OWASP.WebGoat.NET.App_Code.ConfigFile(rooted);

            // Act + Assert
            Assert.Throws<ArgumentException>(() => cfg.Load());
        }
    }
}
