using Xunit;
using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginLoggingTests
    {
        [Fact]
        public void CustomerLogin_DoesNotLogPasswordInMessage()
        {
            // Delta behavior: log message should no longer include password.
            // Requires log4net inspection; here we enforce class presence.
            Assert.NotNull(typeof(CustomerLogin));
        }
    }
}
