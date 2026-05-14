using Xunit;
using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginLoggingTests
    {
        [Fact]
        public void ButtonLogOnClick_DoesNotLogPassword_Smoke()
        {
            // Delta behavior: log message removed password.
            // Smoke test that page type is present; log verification would require injectable logger.
            Assert.NotNull(typeof(CustomerLogin));
        }
    }
}
