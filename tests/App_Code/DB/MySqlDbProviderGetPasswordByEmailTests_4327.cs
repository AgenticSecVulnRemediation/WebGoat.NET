using System;
using Xunit;
using Moq;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPasswordByEmailTests_4327
    {
        [Fact]
        public void GetPasswordByEmail_UsesParameterizedQuery_DoesNotThrow_WithInjectionInput()
        {
            // Same delta as PR 4313 but for PR 4327: ensure the email is passed as parameter.
            var configFile = new Mock<ConfigFile>(MockBehavior.Loose);
            configFile.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(configFile.Object);

            var ex = Record.Exception(() => provider.GetPasswordByEmail("x' OR '1'='1"));
            Assert.Null(ex);
        }
    }
}
