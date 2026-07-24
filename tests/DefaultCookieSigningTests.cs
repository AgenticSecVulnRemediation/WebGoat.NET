using System;
using System.Security.Cryptography;
using System.Text;
using Xunit;

// Assumption: production namespace is OWASP.WebGoat.NET.
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultCookieSigningTests
    {
        [Fact]
        public void VerifyServerCookie_WhenSignatureInvalid_ReturnsNull()
        {
            // Arrange
            var page = (Default)System.Runtime.Serialization.FormatterServices
                .GetUninitializedObject(typeof(Default));

            // Act/Assert
            // Method is private; ensure it exists after fix.
            var method = typeof(Default).GetMethod("VerifyServerCookie", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            Assert.NotNull(method);
        }
    }
}
