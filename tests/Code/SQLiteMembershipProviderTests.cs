using System;
using System.Reflection;
using System.Text.RegularExpressions;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void ValidatePwdStrengthRegularExpression_WithTimeoutEnabled_ThrowsProviderExceptionOnTimeoutProneRegex()
        {
            // Arrange
            // The patch validates the password-strength regex by constructing it with a timeout.
            // We set a known catastrophic-backtracking pattern and assert it is rejected (via ProviderException).
            const string catastrophicPattern = "^(a+)+$";

            var field = typeof(SQLiteMembershipProvider)
                .GetField("_passwordStrengthRegularExpression", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(field);
            field!.SetValue(null, catastrophicPattern);

            var method = typeof(SQLiteMembershipProvider)
                .GetMethod("ValidatePwdStrengthRegularExpression", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(method);

            // Act + Assert
            var ex = Assert.Throws<TargetInvocationException>(() => method!.Invoke(null, null));
            Assert.NotNull(ex.InnerException);
            Assert.Equal("ProviderException", ex.InnerException!.GetType().Name);
        }

        [Fact]
        public void RegexIsMatch_WithTimeout_ThrowsRegexMatchTimeoutExceptionForCatastrophicInput()
        {
            // Arrange
            const string pattern = "^(a+)+$";
            string longInput = new string('a', 50_000) + "!";

            // Act + Assert
            Assert.ThrowsAny<RegexMatchTimeoutException>(() =>
                Regex.IsMatch(longInput, pattern, RegexOptions.None, TimeSpan.FromMilliseconds(100)));
        }
    }
}
