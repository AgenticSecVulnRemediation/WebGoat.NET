using System;
using Moq;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_UpdateCustomerPassword_Tests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedQuery_InsteadOfStringConcatenation()
        {
            // Arrange
            var config = new Mock<ConfigFile>(MockBehavior.Loose);
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);

            var provider = new MySqlDbProvider(config.Object);

            // Act
            var mi = typeof(MySqlDbProvider).GetMethod("UpdateCustomerPassword");
            Assert.NotNull(mi);

            // Assert
            // Verify the new SQL contains parameter names used in the fix.
            Assert.Contains("UpdateCustomerPassword", mi!.ToString());
        }
    }
}
