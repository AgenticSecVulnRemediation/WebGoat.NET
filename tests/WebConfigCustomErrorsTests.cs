using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfigCustomErrorsTests
    {
        [Fact]
        public void WebConfig_CustomErrors_IsRemoteOnly()
        {
            // Arrange
            // Delta test for PR #4004: customErrors changed to RemoteOnly.
            var path = System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "Web.config");

            // Act
            var content = System.IO.File.ReadAllText(path);

            // Assert
            Assert.Contains("<customErrors mode=\"RemoteOnly\"", content);
            Assert.DoesNotContain("<customErrors mode=\"Off\"", content);
        }
    }
}
