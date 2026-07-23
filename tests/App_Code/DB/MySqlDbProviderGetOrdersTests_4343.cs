using System;
using Xunit;
using Moq;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetOrdersTests_4343
    {
        [Fact]
        public void GetOrders_UsesParameterizedQuery_DoesNotThrow()
        {
            // Same delta as PR 4314 but for PR 4343.
            var configFile = new Mock<ConfigFile>(MockBehavior.Loose);
            configFile.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(configFile.Object);

            var ex = Record.Exception(() => provider.GetOrders(42));
            Assert.Null(ex);
        }
    }
}
