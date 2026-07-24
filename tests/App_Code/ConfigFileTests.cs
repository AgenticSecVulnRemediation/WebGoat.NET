using System;
using System.IO;
using Xunit;

// Assumption: production namespace is OWASP.WebGoat.NET.App_Code.
using OWASP.WebGoat.NET.App_Code;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class ConfigFileTests
    {
        [Theory]
        [InlineData("../secrets.txt")]
        [InlineData("..\\secrets.txt")]
        public void Load_WithParentTraversal_Throws(string path)
        {
            // Arrange
            var config = new ConfigFile(path);

            // Act/Assert
            Assert.Throws<Exception>(() => config.Load());
        }

        [Fact]
        public void Load_WithRootedPath_Throws()
        {
            // Arrange
            var rooted = Path.Combine(Path.GetPathRoot(Environment.CurrentDirectory)!, "secret.txt");
            var config = new ConfigFile(rooted);

            // Act/Assert
            Assert.Throws<Exception>(() => config.Load());
        }
    }
}
