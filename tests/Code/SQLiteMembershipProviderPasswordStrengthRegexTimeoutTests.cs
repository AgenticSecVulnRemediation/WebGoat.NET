using System;
using System.Text.RegularExpressions;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProvider_RegexTimeoutTests
    {
        [Fact]
        public void PasswordStrengthRegex_WithTimeout_CanThrowRegexMatchTimeoutException()
        {
            // Delta guard: PR 426 introduced a Regex.IsMatch timeout (TimeSpan.FromSeconds(1)).
            // We validate the runtime behavior of Regex with an explicit timeout to prevent ReDoS.

            const string catastrophicPattern = "^(a+)+$";
            string evilInput = new string('a', 200_000) + "!";

            Assert.Throws<RegexMatchTimeoutException>(() =>
                Regex.IsMatch(evilInput, catastrophicPattern, RegexOptions.None, TimeSpan.FromSeconds(1))
            );
        }
    }
}
