using System;
using System.IO;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class ConfigFileTests_PathTraversalGuard_4317
    {
        [Theory]
        [InlineData("../secrets.config")]
        [InlineData("..\\secrets.config")]
        public void Load_WhenPathContainsTraversal_ThrowsArgumentException(string fileName)
        {
            // Arrange
            var cf = new OWASP.WebGoat.NET.App_Code.ConfigFile(fileName);

            // Act/Assert
            Assert.Throws<ArgumentException>(() => cf.Load());
        }

        [Fact]
        public void Load_WhenPathIsRooted_ThrowsArgumentException()
        {
            // Arrange
            var rooted = Path.Combine(Path.GetPathRoot(Environment.CurrentDirectory)!, "secrets.config");
            var cf = new OWASP.WebGoat.NET.App_Code.ConfigFile(rooted);

            // Act/Assert
            Assert.Throws<ArgumentException>(() => cf.Load());
        }
    }
}
