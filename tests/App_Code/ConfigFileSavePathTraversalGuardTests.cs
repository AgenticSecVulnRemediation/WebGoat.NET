using System;
using Xunit;

// Assumption: production namespace follows folder structure: OWASP.WebGoat.NET.App_Code
using OWASP.WebGoat.NET.App_Code;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class ConfigFileSavePathTraversalGuardTests
    {
        [Theory]
        [InlineData("../secrets.txt")]
        [InlineData("..\\secrets.txt")]
        public void Save_PathContainsTraversal_ThrowsArgumentException(string path)
        {
            // Arrange
            var cfg = new ConfigFile(path);

            // Act/Assert
            Assert.Throws<ArgumentException>(() => cfg.Save());
        }

        [Theory]
        [InlineData("C:\\windows\\system32\\drivers\\etc\\hosts")]
        [InlineData("/etc/passwd")]
        public void Save_PathIsRooted_ThrowsArgumentException(string path)
        {
            // Arrange
            var cfg = new ConfigFile(path);

            // Act/Assert
            Assert.Throws<ArgumentException>(() => cfg.Save());
        }
    }
}
