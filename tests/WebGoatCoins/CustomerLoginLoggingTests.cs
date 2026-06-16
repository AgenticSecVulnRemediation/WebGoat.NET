using System;
using System.IO;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginLoggingTests
    {
        [Fact]
        public void CustomerLoginSource_DoesNotLogPassword()
        {
            // Arrange
            var sourcePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "WebGoatCoins", "CustomerLogin.aspx.cs");
            if (!File.Exists(sourcePath))
            {
                sourcePath = Path.Combine(Directory.GetCurrentDirectory(), "WebGoat", "WebGoatCoins", "CustomerLogin.aspx.cs");
            }

            // Act
            string content = File.ReadAllText(sourcePath);

            // Assert
            Assert.Contains("attempted to log in", content);
            Assert.DoesNotContain("attempted to log in with password", content);
            Assert.DoesNotContain("+ pwd", content);
        }
    }
}
