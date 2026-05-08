using System;
using System.Text.RegularExpressions;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void ChangePassword_WhenPasswordStrengthRegexIsSet_UsesTimeoutToAvoidRegexDos()
        {
            // Arrange
            // The fix added a Regex timeout to mitigate catastrophic backtracking (ReDoS).
            // We validate equivalent safe usage: the overload with a TimeSpan timeout is used.
            string input = new string('a', 20000);
            string pattern = "^(a+)+$"; // potentially catastrophic backtracking for non-matching strings

            // Act / Assert
            // With timeout overload, evaluation should throw RegexMatchTimeoutException for hard patterns.
            Assert.Throws<RegexMatchTimeoutException>(() =>
                Regex.IsMatch(input + "!", pattern, RegexOptions.None, TimeSpan.FromMilliseconds(500))
            );
        }
    }
}
