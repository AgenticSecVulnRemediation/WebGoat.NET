using System;
using System.Text.RegularExpressions;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutDeltaTests
    {
        [Fact]
        public void PasswordStrengthRegex_IsEvaluatedWithTimeout_ToPreventReDoS()
        {
            // Arrange
            // Delta test for PR 4698: Regex.IsMatch now uses an explicit timeout.
            var sourcePath = System.IO.Path.Combine("WebGoat", "Code", "SQLiteMembershipProvider.cs");

            // Act
            var src = System.IO.File.ReadAllText(sourcePath);

            // Assert
            Assert.Contains("Regex.IsMatch(password", src);
            Assert.Contains("TimeSpan.FromMilliseconds(500)", src);

            // Ensure old overload (without timeout) is not used for password strength evaluation
            Assert.DoesNotContain("Regex.IsMatch (password, this.PasswordStrengthRegularExpression)", src);
        }
    }
}
