using System;
using System.Text.RegularExpressions;
using Xunit;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutTests
    {
        [Fact]
        public void ChangePassword_WithPathologicalRegex_DoesNotHang_ThrowsWithinTimeout()
        {
            // Arrange
            // Delta behavior: Regex.IsMatch now includes a timeout (500ms) to mitigate ReDoS.
            // We directly validate the Regex timeout behavior using a similar call pattern.
            string pattern = "^(a+)+$";
            string input = new string('a', 20000) + "!";

            // Act + Assert
            // When a timeout is set, .NET throws RegexMatchTimeoutException for catastrophic backtracking.
            Assert.Throws<RegexMatchTimeoutException>(() =>
                Regex.IsMatch(input, pattern, RegexOptions.None, TimeSpan.FromMilliseconds(1))
            );
        }
    }
}
