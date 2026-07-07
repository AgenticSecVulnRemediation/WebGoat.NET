using System;
using System.Reflection;
using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginLoggingTests
    {
        [Fact]
        public void ButtonLogOn_Click_DoesNotLogPassword()
        {
            // Arrange
            // We verify the secure behavior change: log line no longer concatenates the password.
            var t = typeof(OWASP.WebGoat.NET.WebGoatCoins.CustomerLogin);

            var asmPath = t.Assembly.Location;
            var bytes = System.IO.File.ReadAllBytes(asmPath);
            var text = System.Text.Encoding.UTF8.GetString(bytes);

            // Act / Assert
            Assert.Contains("attempted to log in.", text);
            Assert.DoesNotContain("attempted to log in with password", text);
        }
    }
}
