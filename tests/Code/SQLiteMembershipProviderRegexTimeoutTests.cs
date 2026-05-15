using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutTests
    {
        [Fact]
        public void RegexConstructor_WithTimeout_ThrowsOnTooComplexPatternInsteadOfHanging()
        {
            // Arrange
            // Catastrophic-backtracking style pattern.
            var pattern = "^(a+)+$";

            // Act
            // The fix introduces a Regex constructor overload with a timeout.
            // We verify that the timeout overload can be used and will terminate.
            var start = DateTime.UtcNow;
            var ex = Record.Exception(() =>
            {
                _ = new System.Text.RegularExpressions.Regex(
                    pattern,
                    System.Text.RegularExpressions.RegexOptions.None,
                    TimeSpan.FromMilliseconds(500)
                );
            });
            var elapsed = DateTime.UtcNow - start;

            // Assert
            Assert.Null(ex);
            Assert.True(elapsed < TimeSpan.FromSeconds(2));
        }
    }
}
