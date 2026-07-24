using System;
using Xunit;

namespace WebGoat.Code.Tests
{
    // Assumption: The application project exposes OWASP.WebGoat.NET.IOHelper. If namespaces differ,
    // adjust the using/namespace accordingly.
    public class IOHelperTests
    {
        [Theory]
        [InlineData("../secrets.txt")]
        [InlineData("..\\secrets.txt")]
        [InlineData("..%2fsecrets.txt")]
        public void ReadAllFromFile_PathTraversalLikeInput_ThrowsArgumentException(string path)
        {
            // Arrange / Act
            var ex = Assert.Throws<ArgumentException>(() => OWASP.WebGoat.NET.IOHelper.ReadAllFromFile(path));

            // Assert
            Assert.Equal("Invalid file path", ex.Message);
        }

        [Theory]
        // rooted on Windows
        [InlineData("C:\\Windows\\win.ini")]
        // rooted UNC path
        [InlineData("\\\\server\\share\\file.txt")]
        // rooted on Unix
        [InlineData("/etc/passwd")]
        public void ReadAllFromFile_AbsoluteOrRootedPath_ThrowsArgumentException(string path)
        {
            // Arrange / Act
            var ex = Assert.Throws<ArgumentException>(() => OWASP.WebGoat.NET.IOHelper.ReadAllFromFile(path));

            // Assert
            Assert.Equal("Invalid file path", ex.Message);
        }
    }
}
