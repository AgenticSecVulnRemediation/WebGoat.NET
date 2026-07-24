using System;
using System.IO;
using System.Web;
using Xunit;
using Moq;

// Note: Namespace inference is based on file path; source file doesn't declare a test namespace.
namespace OWASP.WebGoat.NET.Tests.Content
{
    public class PathManipulationTests
    {
        [Theory]
        [InlineData("../web.config")]
        [InlineData("..\\web.config")]
        public void ResponseFile_PathTraversalInFullPath_ReturnsFalse(string fullPath)
        {
            // Arrange
            var request = new Mock<HttpRequest>(MockBehavior.Loose);
            var response = new Mock<HttpResponse>(MockBehavior.Loose);

            // Act
            var result = OWASP.WebGoat.NET.PathManipulation.ResponseFile(request.Object, response.Object, "web.config", fullPath, 100);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ResponseFile_PathTraversalInAbsolutePathOutsideAllowedBase_ReturnsFalse()
        {
            // Arrange
            var request = new Mock<HttpRequest>(MockBehavior.Loose);
            var response = new Mock<HttpResponse>(MockBehavior.Loose);

            // Absolute rooted path will trigger (IsPathRooted && !StartsWith("<allowedBasePath>"))
            var rooted = Path.Combine(Path.GetPathRoot(Environment.CurrentDirectory) ?? "C:\\", "temp", "file.txt");

            // Act
            var result = OWASP.WebGoat.NET.PathManipulation.ResponseFile(request.Object, response.Object, "file.txt", rooted, 100);

            // Assert
            Assert.False(result);
        }
    }
}
