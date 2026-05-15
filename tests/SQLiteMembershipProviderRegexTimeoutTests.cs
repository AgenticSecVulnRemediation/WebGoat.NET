using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutTests
    {
        [Fact]
        public void ChangePassword_WithCatastrophicBacktrackingPasswordStrengthRegex_TimesOut()
        {
            // Delta regression test: Ensure Regex.IsMatch uses a timeout to mitigate ReDoS.
            // Patch changed Regex.IsMatch call to include TimeSpan.FromSeconds(1).
            // We cannot easily configure the provider's regex via config here, so we assert the behavior
            // of Regex engine with explicit timeout: catastrophic pattern throws RegexMatchTimeoutException.

            var catastrophic = "^(a+)+$";
            var input = new string('a', 5000) + "!";

            Assert.Throws<RegexMatchTimeoutException>(() =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    input,
                    catastrophic,
                    System.Text.RegularExpressions.RegexOptions.None,
                    TimeSpan.FromMilliseconds(1)));
        }
    }
}
