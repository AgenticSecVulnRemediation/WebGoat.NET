using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class UtilTests_PathTraversalGuard
    {
        [Fact]
        public void RunProcessWithInput_WhenInputIsRootedPath_ThrowsArgumentException()
        {
            // Arrange
            var rooted = Path.GetPathRoot(Environment.CurrentDirectory) + "temp.txt";

            // Act/Assert
            Assert.Throws<ArgumentException>(() => OWASP.WebGoat.NET.App_Code.Util.RunProcessWithInput("cmd", "", rooted));
        }

        [Fact]
        public void RunProcessWithInput_WhenInputContainsTraversal_ThrowsArgumentException()
        {
            // Arrange
            var traversal = "../secrets.txt";

            // Act/Assert
            Assert.Throws<ArgumentException>(() => OWASP.WebGoat.NET.App_Code.Util.RunProcessWithInput("cmd", "", traversal));
        }
    }
}
