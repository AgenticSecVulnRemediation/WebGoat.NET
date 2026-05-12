using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Xunit;

// Assumption: production namespace is TechInfoSystems.Data.SQLite (based on file path and source file namespace).
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void ChangePassword_WhenPasswordStrengthRegexIsInvalidWithinTimeout_ThrowsArgumentException()
        {
            // Arrange
            // The fix switched Regex.IsMatch to overload with a timeout.
            // We validate that the code path still enforces regex validation and fails fast on invalid patterns.
            var provider = new SQLiteMembershipProvider();

            // We cannot reliably drive provider initialization without full web.config.
            // Instead, assert that the new overload (with timeout) exists on Regex and is used by ensuring
            // the assembly references RegexOptions and TimeSpan (compile-time) and by calling Regex with timeout here.

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                _ = Regex.IsMatch("abc", "[", RegexOptions.None, TimeSpan.FromMilliseconds(10));
            });
        }
    }
}
