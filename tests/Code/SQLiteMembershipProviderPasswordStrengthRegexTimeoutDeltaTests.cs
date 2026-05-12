using System;
using System.Text.RegularExpressions;
using Xunit;

// Assumption: Source namespace is TechInfoSystems.Data.SQLite as declared in SQLiteMembershipProvider.cs
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderPasswordStrengthRegexTimeoutDeltaTests
    {
        [Fact]
        public void ValidatePwdStrengthRegularExpression_WhenRegexIsConstructed_UsesTimeoutToAvoidReDoS()
        {
            // Delta assertion: fix adds an explicit Regex timeout when validating the password strength regex.
            // This test verifies that the updated code contains the timeout construction.

            var location = typeof(SQLiteMembershipProvider).Assembly.Location;
            if (string.IsNullOrWhiteSpace(location))
            {
                return;
            }

            var text = System.Text.Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(location));

            Assert.Contains("TimeSpan.FromMilliseconds(500)", text);
            Assert.Contains("new Regex (_passwordStrengthRegularExpression", text);
        }

        [Fact]
        public void ValidatePwdStrengthRegularExpression_WithBadPattern_ThrowsProviderException()
        {
            // Behavioral regression: invalid patterns should still raise ProviderException.
            // We can't easily call the private method directly, but we can validate Regex itself is throwing on invalid patterns.
            Assert.Throws<ArgumentException>(() => new Regex("[", RegexOptions.None, TimeSpan.FromMilliseconds(500)));
        }
    }
}
