using Xunit;
using Moq;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests_CustomCustomerLogin
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterForEmail_InsteadOfConcatenation()
        {
            // Arrange
            var config = new Mock<ConfigFile>(MockBehavior.Loose);
            config.Setup(c => c.Get(It.IsAny<string>())).Returns("test.db");

            var provider = new SqliteDbProvider(config.Object);

            // Act
            string sql = "select * from CustomerLogin where email = @Email;";

            // Assert
            Assert.Contains("@Email", sql);
            Assert.DoesNotContain("email = '" + " +", sql);
        }
    }
}
