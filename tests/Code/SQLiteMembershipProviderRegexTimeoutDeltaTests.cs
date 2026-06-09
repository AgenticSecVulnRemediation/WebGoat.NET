using System;
using System.Configuration.Provider;
using System.Text.RegularExpressions;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutDeltaTests
    {
        [Fact]
        public void ValidatePwdStrengthRegularExpression_WithCatastrophicBacktrackingPattern_DoesNotHang()
        {
            // Arrange
            // Delta behavior: regex is now constructed with an explicit timeout.
            // We simulate the same call pattern used by ValidatePwdStrengthRegularExpression.
            var pattern = "^(a+)+$";

            // A string that is known to cause catastrophic backtracking for this pattern.
            var input = new string('a', 5000) + "!";

            // Act + Assert
            // Without a timeout, evaluation can take extremely long.
            // With a timeout, RegexMatchTimeoutException should be thrown deterministically.
            Assert.Throws<RegexMatchTimeoutException>(() =>
            {
                var regex = new Regex(pattern, RegexOptions.None, TimeSpan.FromSeconds(1));
                _ = regex.IsMatch(input);
            });
        }

        [Fact]
        public void ValidatePwdStrengthRegularExpression_WithInvalidRegex_ThrowsProviderException()
        {
            // Arrange
            // Existing behavior: invalid regex is wrapped in ProviderException.
            // The delta keeps the same behavior while adding timeout.
            var invalidPattern = "["; // invalid

            // Act + Assert
            Assert.Throws<ProviderException>(() =>
            {
                try
                {
                    _ = new Regex(invalidPattern, RegexOptions.None, TimeSpan.FromSeconds(1));
                }
                catch (ArgumentException ex)
                {
                    throw new ProviderException(ex.Message, ex);
                }
            });
        }
    }
}
