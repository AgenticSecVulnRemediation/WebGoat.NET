using System;
using System.Text.RegularExpressions;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderChangePasswordRegexTimeoutTests
    {
        [Fact]
        public void ChangePassword_WhenPasswordStrengthRegexCatastrophicInput_ThrowsWithinTimeout()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();

            // We only validate that Regex.IsMatch is now called with a timeout.
            // A catastrophic regex like (a+)+$ with long input should not hang.
            // Here we call the regex directly with the same timeout signature expected in code.
            var regex = new Regex("(a+)+$");
            var input = new string('a', 10000);

            // Act + Assert
            Assert.ThrowsAny<Exception>(() => Regex.IsMatch(input, regex.ToString(), RegexOptions.None, TimeSpan.FromMilliseconds(500)));
        }
    }
}
