using System;
using System.IO;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderCustomCustomerLoginTests
    {
        [Fact]
        public void CustomCustomerLogin_WithUnknownEmail_ReturnsEmailNotFoundMessage()
        {
            // Arrange
            string tempFile = Path.GetTempFileName();
            try
            {
                var config = new Mock<ConfigFile>(MockBehavior.Loose);
                config.Setup(c => c.Get(DbConstants.KEY_FILE_NAME)).Returns(tempFile);
                config.Setup(c => c.Get(It.Is<string>(k => k != DbConstants.KEY_FILE_NAME))).Returns(string.Empty);

                var provider = new SqliteDbProvider(config.Object);

                // Act
                string result = provider.CustomCustomerLogin("nobody@example.com", "pw");

                // Assert
                Assert.Equal("Email Address Not Found!", result);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }
    }
}
