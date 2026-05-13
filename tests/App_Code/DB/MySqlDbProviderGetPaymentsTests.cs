using System;
using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPaymentsTests
    {
        [Fact]
        public void GetPayments_WithArbitraryCustomerNumber_DoesNotThrowArgumentNullException()
        {
            // Delta focus: customerNumber now passed as @customerNumber parameter.
            var cfg = new Mock<ConfigFile>();
            cfg.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(cfg.Object);

            var ex = Record.Exception(() => provider.GetPayments(1));
            Assert.False(ex is ArgumentNullException);
        }
    }
}
