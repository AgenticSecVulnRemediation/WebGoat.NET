using System;
using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByCustomerNumberTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_WithSqlLikeNumberInput_DoesNotThrowArgumentNullException()
        {
            // Delta focus: ExecuteScalar now uses query with @num parameter.
            var cfg = new Mock<ConfigFile>();
            cfg.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(cfg.Object);

            var ex = Record.Exception(() => provider.GetEmailByCustomerNumber("1 OR 1=1"));
            Assert.False(ex is ArgumentNullException);
        }
    }
}
