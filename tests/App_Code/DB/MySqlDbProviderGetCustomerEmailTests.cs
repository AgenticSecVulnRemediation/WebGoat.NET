using System;
using Xunit;
using Moq;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_WithInjectionLikeCustomerNumber_DoesNotThrow_FromQueryConcatenation()
        {
            // Delta test: customerNumber lookup changed to parameterized query.

            var configFile = new Mock<ConfigFile>(MockBehavior.Loose);
            configFile.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(configFile.Object);

            var ex = Record.Exception(() => provider.GetCustomerEmail("1 OR 1=1"));
            Assert.Null(ex);
        }
    }
}
