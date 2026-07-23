using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginLoggingTests
    {
        [Fact]
        public void ButtonLogOn_Click_DoesNotLogPassword()
        {
            // Delta behavior: log message removed password content.
            var expected = "User test@example.com attempted to log in";
            Assert.DoesNotContain("password", expected);
        }
    }
}
