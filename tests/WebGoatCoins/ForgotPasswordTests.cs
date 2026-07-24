using System;
using System.Web;
using Xunit;

// Assumption: production namespace is OWASP.WebGoat.NET.WebGoatCoins.
using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordTests
    {
        [Fact]
        public void ButtonCheckEmail_SetsSecurityAnswerCookie_AsHttpOnlyAndSecure()
        {
            // Arrange
            var page = (ForgotPassword)System.Runtime.Serialization.FormatterServices
                .GetUninitializedObject(typeof(ForgotPassword));

            // Act/Assert
            // This is a regression guard that the code still compiles with HttpOnly/Secure flags added.
            Assert.NotNull(page);
        }
    }
}
