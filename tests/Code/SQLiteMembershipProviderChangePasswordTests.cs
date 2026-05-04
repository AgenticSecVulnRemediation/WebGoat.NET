using System;
using System.Collections.Specialized;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderChangePasswordTests
    {
        [Fact]
        public void ChangePassword_WithCatastrophicBacktrackingRegex_TimesOutInsteadOfHanging()
        {
            // Arrange
            // We want to exercise the changed overload: Regex.IsMatch(..., TimeSpan.FromMilliseconds(1000))
            // To avoid the provider requiring web.config/DB initialization, set the private static field directly.
            var provider = new SQLiteMembershipProvider();

            var regexField = typeof(SQLiteMembershipProvider)
                .GetField("_passwordStrengthRegularExpression", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(regexField);

            // A classic catastrophic-backtracking regex.
            regexField!.SetValue(null, "^(a+)+$");

            // Also set minimal length/non-alphanumeric requirements to values that won't fail first.
            typeof(SQLiteMembershipProvider).GetField("_minRequiredPasswordLength", BindingFlags.NonPublic | BindingFlags.Static)!
                .SetValue(null, 1);
            typeof(SQLiteMembershipProvider).GetField("_minRequiredNonAlphanumericCharacters", BindingFlags.NonPublic | BindingFlags.Static)!
                .SetValue(null, 0);

            // Act + Assert
            // This input triggers catastrophic backtracking. With the fix, it should throw RegexMatchTimeoutException.
            var ex = Assert.Throws<RegexMatchTimeoutException>(() =>
                provider.ChangePassword("user", "old", new string('a', 30000) + "!"));

            Assert.Contains("The password does not match", ex.Message);
        }
    }
}
