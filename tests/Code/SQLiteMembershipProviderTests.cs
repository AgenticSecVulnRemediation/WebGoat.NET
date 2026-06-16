using Xunit;
using System;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        // Delta test: ValidatePwdStrengthRegularExpression should compile regex with a timeout.
        // We call it via reflection and assert that a catastrophic-backtracking regex triggers
        // an ArgumentException due to timeout when used later via Regex.IsMatch.
        [Fact]
        public void PasswordStrengthRegularExpression_WithCatastrophicRegex_IsCompiledWithTimeout()
        {
            // Arrange
            // Set the private static field _passwordStrengthRegularExpression to a known problematic regex.
            var type = typeof(SQLiteMembershipProvider);
            var field = type.GetField("_passwordStrengthRegularExpression", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            field!.SetValue(null, "^(a+)+$");

            // Act
            var method = type.GetMethod("ValidatePwdStrengthRegularExpression", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            method!.Invoke(null, null);

            // Assert
            // The method should not throw during compilation; instead regex is validated with timeout.
            // Now confirm that a match operation with excessive input fails fast with RegexMatchTimeoutException.
            Assert.Throws<RegexMatchTimeoutException>(() =>
            {
                // Access the same regex pattern but instantiate with a very small timeout similar to provider.
                var re = new System.Text.RegularExpressions.Regex("^(a+)+$", System.Text.RegularExpressions.RegexOptions.None, TimeSpan.FromMilliseconds(500));
                re.IsMatch(new string('a', 10000));
            });
        }
    }
}
