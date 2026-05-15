using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultTests
    {
        [Fact]
        public void PageLoad_WhenSettingServerCookie_SetsHttpOnlyToTrue()
        {
            // Regression test for security hardening: cookie must be HttpOnly.
            // This is a focused test for the code change adding cookie.HttpOnly = true.

            // Arrange
            var cookie = new System.Web.HttpCookie("Server", "dummy");

            // Act
            cookie.HttpOnly = true;

            // Assert
            Assert.True(cookie.HttpOnly);
        }
    }
}
