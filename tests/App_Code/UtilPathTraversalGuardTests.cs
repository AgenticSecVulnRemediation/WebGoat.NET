using System;
using System.IO;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class Util_RunProcessWithInput_PathTraversalGuardTests
    {
        [Theory]
        [InlineData("..\\evil.sql")]
        [InlineData("../evil.sql")]
        public void RunProcessWithInput_InputContainsTraversal_ThrowsArgumentException(string input)
        {
            // Arrange
            // We should fail fast before any file IO / process launching based on the new guard.

            // Act + Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                OWASP.WebGoat.NET.App_Code.Util.RunProcessWithInput("cmd", "args", input));

            Assert.Contains("Invalid file path", ex.Message);
        }

        [Fact]
        public void RunProcessWithInput_InputIsRootedPath_ThrowsArgumentException()
        {
            // Arrange
            // Cross-platform rooted path example.
            var rooted = Path.IsPathRooted("/tmp/evil.sql") ? "/tmp/evil.sql" : Path.GetFullPath("evil.sql");

            // Act + Assert
            Assert.Throws<ArgumentException>(() =>
                OWASP.WebGoat.NET.App_Code.Util.RunProcessWithInput("cmd", "args", rooted));
        }
    }
}
