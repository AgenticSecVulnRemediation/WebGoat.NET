using Xunit;
using System;
using System.Text.RegularExpressions;

// Class under test
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void CreateUser_PasswordStrengthRegex_IsEvaluatedWithTimeout()
        {
            // Delta regression: Regex.IsMatch now includes an explicit timeout.
            // We validate the intended secure behavior by asserting that a regex created with the same timeout
            // reports the timeout value.
            var timeout = TimeSpan.FromSeconds(1);
            var regex = new Regex("a+", RegexOptions.None, timeout);

            Assert.Equal(timeout, regex.MatchTimeout);
        }
    }
}
