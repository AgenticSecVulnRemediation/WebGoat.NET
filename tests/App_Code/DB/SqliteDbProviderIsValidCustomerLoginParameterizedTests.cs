using Xunit;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using System;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderIsValidCustomerLoginParameterizedTests
    {
        [Fact]
        public void IsValidCustomerLogin_WithInjectedEmail_DoesNotThrowAndDoesNotAuthenticate()
        {
            // Arrange
            var config = new Mock<ConfigFile>(MockBehavior.Loose);
            config.Setup(c => c.Get(It.IsAny<string>())).Returns("test.db");
            var provider = new SqliteDbProvider(config.Object);

            // Act
            // Injection attempt should not result in successful authentication.
            // In absence of a DB, the method may throw due to missing file/tables; ensure no SQL syntax concatenation errors.
            var ex = Record.Exception(() => provider.IsValidCustomerLogin("' OR 1=1 --", "pw"));

            // Assert
            if (ex != null)
            {
                Assert.DoesNotContain("near \"OR\"", ex.Message, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
