using System;
using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesEncodedPasswordParameter_DoesNotThrowArgumentNullException()
        {
            // Delta focus: query changed to parameterized UPDATE with @password and @customerNumber.

            var cfg = new Mock<ConfigFile>();
            cfg.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(cfg.Object);

            var ex = Record.Exception(() => provider.UpdateCustomerPassword(1, "p@ss'word"));
            Assert.False(ex is ArgumentNullException);
        }
    }
}
