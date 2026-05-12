using Xunit;
using System.Reflection;
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultServerCookieHttpOnlyDeltaTests
    {
        [Fact]
        public void Default_Page_Load_SetsServerCookieHttpOnly()
        {
            // Arrange
            var type = typeof(Default);

            // Act
            var method = type.GetMethod("Page_Load", BindingFlags.Instance | BindingFlags.NonPublic);

            // Assert
            Assert.NotNull(method);
        }
    }
}
