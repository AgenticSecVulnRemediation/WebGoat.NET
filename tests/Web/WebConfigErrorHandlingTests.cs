using Xunit;

namespace WebGoat.Web.Tests
{
    public class WebConfigErrorHandlingTests
    {
        [Fact]
        public void WebConfig_CustomErrors_IsRemoteOnly()
        {
            // Arrange
            var webConfig = System.IO.File.ReadAllText("WebGoat/Web.config");

            // Act / Assert
            Assert.Contains("<customErrors", webConfig);
            Assert.Contains("mode=\"RemoteOnly\"", webConfig);
        }
    }
}
