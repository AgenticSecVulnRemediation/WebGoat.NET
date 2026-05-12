using System;
using System.IO;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderIsValidCustomerLoginTests
    {
        [Fact]
        public void IsValidCustomerLogin_WithNoRows_ReturnsTrueAndDoesNotThrow()
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
                bool result = provider.IsValidCustomerLogin("user@example.com", "pw");

                // Assert
                // Note: this asserts current behavior (Rows.Count == 0). If this is unintended, PR should be revisited.
                Assert.True(result);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }
    }
}
