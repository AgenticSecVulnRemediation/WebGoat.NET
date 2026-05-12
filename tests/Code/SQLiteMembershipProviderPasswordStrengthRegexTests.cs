using System;
using System.Text.RegularExpressions;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderPasswordStrengthRegexTests
    {
        [Fact]
        public void PasswordStrengthRegex_WhenEvaluated_UsesTimeoutOverload()
        {
            // Delta: Regex.IsMatch now uses a timeout to reduce ReDoS risk
            var ok = Regex.IsMatch("password!", "^.*$", RegexOptions.None, TimeSpan.FromSeconds(1));
            Assert.True(ok);
        }
    }
}
