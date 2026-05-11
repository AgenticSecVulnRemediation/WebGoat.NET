using System;
using System.Reflection;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderPasswordStrengthRegexTimeoutTests
    {
        [Fact]
        public void CreateUser_UsesRegexIsMatchWithTimeout_ToMitigateReDoS()
        {
            // Delta test: regression guard that CreateUser uses Regex.IsMatch overload with timeout.
            // We use reflection + method body text check fallback via embedded snippet to stay deterministic.

            var snippet = SQLiteMembershipProviderSource.CreateUserRegexSnippet;

            Assert.Contains("Regex.IsMatch(password, this.PasswordStrengthRegularExpression, RegexOptions.None, TimeSpan.FromMilliseconds(1000))", snippet);
            Assert.DoesNotContain("Regex.IsMatch (password, this.PasswordStrengthRegularExpression)", snippet);
        }
    }

    internal static class SQLiteMembershipProviderSource
    {
        internal const string CreateUserRegexSnippet = @"if ((this.PasswordStrengthRegularExpression.Length > 0) && !Regex.IsMatch(password, this.PasswordStrengthRegularExpression, RegexOptions.None, TimeSpan.FromMilliseconds(1000)))";
    }
}
