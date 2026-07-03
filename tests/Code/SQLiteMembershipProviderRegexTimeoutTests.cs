using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Xunit;

// Assumptions:
// - Source namespace is TechInfoSystems.Data.SQLite (from WebGoat/Code/SQLiteMembershipProvider.cs)
// - The project references System.Web and Mono.Data.Sqlite at build time; this test avoids DB access.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutTests
    {
        [Fact]
        public void ChangePassword_PasswordStrengthRegexUsesTimeout_ThrowsArgumentExceptionQuickly()
        {
            // Arrange
            // Configure provider's static regex to a catastrophic pattern.
            SetStaticField("_passwordStrengthRegularExpression", "^(a+)+$");
            SetStaticField("_minRequiredPasswordLength", 1);
            SetStaticField("_minRequiredNonAlphanumericCharacters", 0);

            var provider = new SQLiteMembershipProvider();

            // Use an input that would take extremely long without a timeout.
            var evilPassword = new string('a', 20000) + "!";

            // Act + Assert
            // After the fix, ChangePassword uses Regex.IsMatch with a timeout, which should result in an exception
            // rather than potentially hanging the test process.
            Assert.ThrowsAny<ArgumentException>(() => provider.ChangePassword("user", "old", evilPassword));
        }

        private static void SetStaticField(string fieldName, object value)
        {
            var field = typeof(SQLiteMembershipProvider).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(field);
            field!.SetValue(null, value);
        }
    }
}
