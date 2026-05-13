using System;
using System.Web.Security;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultPageCookieProtectionTests
    {
        [Fact]
        public void ProtectAndUnprotect_RoundTripsCookieValue()
        {
            // Delta behavior: cookie value now protected with MachineKey.Protect.
            var original = "server-name";
            var data = System.Text.Encoding.UTF8.GetBytes(original);

            var protectedBytes = MachineKey.Protect(data);
            Assert.NotNull(protectedBytes);
            Assert.NotEmpty(protectedBytes);

            var unprotected = MachineKey.Unprotect(protectedBytes);
            Assert.NotNull(unprotected);
            Assert.Equal(original, System.Text.Encoding.UTF8.GetString(unprotected));
        }
    }
}
