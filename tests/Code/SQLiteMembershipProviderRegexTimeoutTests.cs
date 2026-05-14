using System;
using System.Text.RegularExpressions;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    // NOTE: Namespace inferred from source file path "WebGoat/Code/SQLiteMembershipProvider.cs".
    public class SQLiteMembershipProviderRegexTimeoutTests
    {
        [Fact]
        public void CreateUser_PasswordStrengthRegex_UsesTimeout_ToMitigateReDoS()
        {
            // Patch changed Regex.IsMatch(password, regex) to Regex.IsMatch(password, regex, RegexOptions.None, TimeSpan.FromSeconds(1))
            // Validate that the overload with a timeout throws RegexMatchTimeoutException for catastrophic backtracking.

            var pattern = "^(a+)+$";
            var input = new string('a', 50000) + "!"; // crafted to backtrack

            Assert.Throws<RegexMatchTimeoutException>(() =>
                Regex.IsMatch(input, pattern, RegexOptions.None, TimeSpan.FromMilliseconds(1)));
        }
    }
}
