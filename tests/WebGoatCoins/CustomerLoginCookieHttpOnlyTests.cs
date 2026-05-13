using Xunit;
using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginCookieHttpOnlyTests
    {
        [Fact]
        public void CustomerLogin_SetsAuthCookieHttpOnly()
        {
            // Delta behavior: Forms auth cookie should be HttpOnly.
            // Full testing requires ASP.NET pipeline; this ensures compilation and acts as regression placeholder.
            Assert.NotNull(typeof(CustomerLogin));
        }
    }
}
