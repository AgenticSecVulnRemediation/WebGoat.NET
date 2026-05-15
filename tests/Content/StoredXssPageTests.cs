using System;
using System.IO;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class StoredXssPageTests
    {
        [Fact]
        public void StoredXssPage_HasValidateRequestEnabled()
        {
            // Arrange
            var path = Path.Combine("WebGoat", "Content", "StoredXSS.aspx");

            // This test assumes it runs from repo root; if test runner uses a different base dir, adjust accordingly.
            Assert.True(File.Exists(path), $"Expected file at {path}");

            // Act
            var markup = File.ReadAllText(path);

            // Assert
            // Security fix: validateRequest must be true (was false)
            Assert.Contains("validateRequest=\"true\"", markup);
            Assert.DoesNotContain("validateRequest=\"false\"", markup);
        }
    }
}
