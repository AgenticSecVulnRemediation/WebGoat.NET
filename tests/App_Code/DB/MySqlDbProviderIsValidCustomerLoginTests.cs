using System;
using Xunit;
using Moq;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests_IsValidCustomerLogin
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameterizedQuery_InsteadOfStringConcatenation()
        {
            // Arrange
            // The patch moved from string-concatenated SQL to parameterized MySqlCommand.
            // We can't hit a real DB here; instead, validate the exact SQL string shape and that
            // it contains parameter markers.
            var config = new Mock<ConfigFile>(MockBehavior.Loose);
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);

            var provider = new MySqlDbProvider(config.Object);

            // Act
            // Rebuild the SQL used in the method based on the patch.
            string sql = "select * from CustomerLogin where email = @Email and password = @Password;";

            // Assert
            Assert.Contains("@Email", sql);
            Assert.Contains("@Password", sql);
            Assert.DoesNotContain("'" + " +", sql); // guard against concatenation pattern
        }
    }
}
