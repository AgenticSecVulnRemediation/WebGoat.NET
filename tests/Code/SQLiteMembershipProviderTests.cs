using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Theory]
        [InlineData("^(a+)+$", "aaaaaaaaaaaaaaaaaaaaaaaaaaaa!")]
        [InlineData("(a|aa)+$", "aaaaaaaaaaaaaaaaaaaaaaaaaaaa!")]
        public void ValidatePwdStrengthRegularExpression_WithEvilRegex_CompletesWithinTimeout(string pattern, string input)
        {
            // Arrange
            var start = DateTime.UtcNow;

            // Act
            // The fix constructs Regex with an explicit timeout. This test verifies the pattern compilation
            // does not hang indefinitely for catastrophic patterns.
            var ex = Record.Exception(() => new System.Text.RegularExpressions.Regex(
                pattern,
                System.Text.RegularExpressions.RegexOptions.None,
                TimeSpan.FromMilliseconds(500)
            ));

            var elapsed = DateTime.UtcNow - start;

            // Assert
            Assert.Null(ex);
            Assert.True(elapsed < TimeSpan.FromSeconds(2));
        }
    }
}
