using System;
using System.Text.RegularExpressions;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutTests
    {
        [Fact]
        public void RegexIsMatch_WithTimeout_ThrowsRegexMatchTimeoutException_OnCatastrophicBacktracking()
        {
            // Arrange
            // Catastrophic pattern and a near-match input; without a timeout this can take very long.
            var pattern = "^(a+)+$";
            var input = new string('a', 50_000) + "!";

            // Act/Assert
            Assert.Throws<RegexMatchTimeoutException>(() =>
                Regex.IsMatch(input, pattern, RegexOptions.None, TimeSpan.FromMilliseconds(1)));
        }
    }
}
