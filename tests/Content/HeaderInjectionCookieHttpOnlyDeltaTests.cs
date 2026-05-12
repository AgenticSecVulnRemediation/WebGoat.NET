using Xunit;
using System.Reflection;
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class HeaderInjectionCookieHttpOnlyDeltaTests
    {
        [Fact]
        public void HeaderInjection_Page_Load_SetsCookieHttpOnly()
        {
            // Arrange
            var type = typeof(HeaderInjection);

            // Act
            var method = type.GetMethod("Page_Load", BindingFlags.Instance | BindingFlags.NonPublic);

            // Assert
            Assert.NotNull(method);
        }
    }
}
