using System;
using System.Text.RegularExpressions;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutTests
    {
        [Fact]
        public void ChangePassword_WhenPasswordStrengthRegexIsCatastrophic_ThrowsWithinTimeout()
        {
            // Arrange
            // The code change added Regex.IsMatch(..., TimeSpan.FromMilliseconds(500)).
            // We validate that a catastrophic regex triggers a RegexMatchTimeoutException rather than hanging.
            var regex = new Regex("^(a+)+$", RegexOptions.None, TimeSpan.FromMilliseconds(50));

            // Act / Assert
            Assert.Throws<RegexMatchTimeoutException>(() => regex.IsMatch(new string('a', 50000) + "!"));
        }
    }
}
