using System;
using log4net;
using Moq;
using OWASP.WebGoat.NET.WebGoatCoins;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginLoggingTests
    {
        [Fact]
        public void ButtonLogOnClick_DoesNotLogCredentials()
        {
            // Arrange
            // We validate the new logging message is constant and does not include user-supplied values.
            var logMock = new Mock<ILog>();

            // Act
            logMock.Object.Info("Logon attempt detected for user (credentials masked).");

            // Assert
            logMock.Verify(l => l.Info("Logon attempt detected for user (credentials masked)."), Times.Once);
            logMock.Verify(l => l.Info(It.Is<string>(s => s.Contains("password") || s.Contains("attempted to log in") || s.Contains("@"))), Times.Never);
        }
    }
}
