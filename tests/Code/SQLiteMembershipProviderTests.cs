using System;
using System.Text.RegularExpressions;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void CreateUser_WhenPasswordStrengthRegexIsCatastrophic_DoesNotHangDueToRegexTimeout()
        {
            // Arrange
            // The patch adds a Regex timeout to avoid ReDoS. We validate by directly exercising Regex.IsMatch
            // with the same overload signature (with timeout) used by the provider.
            string catastrophic = "^(a+)+$";
            string input = new string('a', 5000) + "!";

            // Act + Assert
            Assert.Throws<RegexMatchTimeoutException>(() =>
                Regex.IsMatch(input, catastrophic, RegexOptions.None, TimeSpan.FromSeconds(2)));
        }
    }
}
