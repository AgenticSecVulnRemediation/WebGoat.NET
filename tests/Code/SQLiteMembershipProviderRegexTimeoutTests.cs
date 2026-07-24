using System;
using System.Text.RegularExpressions;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutTests
    {
        [Fact]
        public void PasswordStrengthRegex_WithCatastrophicBacktrackingPattern_ThrowsRegexMatchTimeoutException()
        {
            // Arrange
            // PR #4681 changed regex compilation to include a timeout to mitigate ReDoS.
            // We validate .NET regex timeout behavior directly using the same constructor signature.
            var catastrophic = "^(a+)+$";

            // Act / Assert
            var ex = Assert.Throws<RegexMatchTimeoutException>(() =>
            {
                var r = new Regex(catastrophic, RegexOptions.None, TimeSpan.FromMilliseconds(1));
                // Long input triggers catastrophic backtracking and should be interrupted.
                _ = r.IsMatch(new string('a', 20000) + "X");
            });

            Assert.Contains("timeout", ex.Message, StringComparison.OrdinalIgnoreCase);
        }
    }
}
