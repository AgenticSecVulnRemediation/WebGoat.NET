using System;
using System.IO;
using Xunit;

using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class IOHelperTests
    {
        [Theory]
        [InlineData("../secrets.txt")]
        [InlineData("..\\secrets.txt")]
        public void ReadAllFromFile_PathTraversalAttempt_ThrowsArgumentException(string path)
        {
            // Arrange / Act / Assert
            Assert.Throws<ArgumentException>(() => IOHelper.ReadAllFromFile(path));
        }

        [Fact]
        public void ReadAllFromFile_RootedPath_ThrowsArgumentException()
        {
            // Arrange
            var rooted = Path.GetTempFileName();

            // Act / Assert
            Assert.Throws<ArgumentException>(() => IOHelper.ReadAllFromFile(rooted));
        }
    }
}
