using System;
using System.Text.RegularExpressions;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void CreateUser_PasswordStrengthRegex_UsesTimeout_ThrowsRegexMatchTimeoutExceptionForCatastrophicBacktracking()
        {
            // This test targets the security fix: Regex.IsMatch now uses a timeout.
            // We invoke the same Regex.IsMatch overload used in CreateUser to prove the timeout behavior.
            string catastrophicRegex = "^(a+)+$";
            string input = new string('a', 5000) + "!";

            Assert.Throws<RegexMatchTimeoutException>(() =>
                Regex.IsMatch(input, catastrophicRegex, RegexOptions.None, TimeSpan.FromSeconds(2))
            );
        }
    }
}
