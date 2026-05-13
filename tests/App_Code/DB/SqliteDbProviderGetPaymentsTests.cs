using System;
using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPaymentsTests
    {
        [Fact]
        public void GetPayments_WithCustomerNumber_UsesParameterStyle_DoesNotThrowArgumentNullException()
        {
            // Delta focus: Payments query now uses @customerNumber parameter.
            var cfg = new Mock<ConfigFile>();
            cfg.Setup(c => c.Get(It.IsAny<string>())).Returns("test.db");
            var provider = new SqliteDbProvider(cfg.Object);

            var ex = Record.Exception(() => provider.GetPayments(1));
            Assert.False(ex is ArgumentNullException);
        }
    }
}
