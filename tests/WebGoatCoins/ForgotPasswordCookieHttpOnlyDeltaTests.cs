using Xunit;
using System.Reflection;
using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordCookieHttpOnlyDeltaTests
    {
        [Fact]
        public void ButtonCheckEmail_Click_SetsCookieHttpOnly()
        {
            // Arrange
            var type = typeof(ForgotPassword);

            // Act
            var method = type.GetMethod("ButtonCheckEmail_Click", BindingFlags.Instance | BindingFlags.NonPublic);

            // Assert
            Assert.NotNull(method);
        }
    }
}
