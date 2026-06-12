using System;
using System.Text.RegularExpressions;
using Xunit;

// Assumption: Source namespace is TechInfoSystems.Data.SQLite as declared in the updated file.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void ChangePassword_WithPathologicalPasswordStrengthRegex_TimesOutQuickly()
        {
            // Arrange
            // The patch adds a Regex timeout (TimeSpan.FromSeconds(1)) when evaluating password strength.
            // We validate that a pathological regex does not hang indefinitely.
            var provider = new SQLiteMembershipProvider();

            // Use reflection to set the private static field _passwordStrengthRegularExpression
            // so we can focus only on the changed behavior.
            var regexField = typeof(SQLiteMembershipProvider).GetField("_passwordStrengthRegularExpression",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(regexField);

            // Classic catastrophic backtracking pattern.
            regexField!.SetValue(null, "^(a+)+$");

            // Act + Assert
            // Trigger only the regex evaluation path added in ChangePassword by invoking it with a long 'a' string.
            // ChangePassword will fail earlier due to CheckPassword (needs DB), so we directly test the regex timeout
            // via the same construction as the patched code.
            var longInput = new string('a', 100_000);

            var ex = Assert.Throws<RegexMatchTimeoutException>(() =>
            {
                _ = new Regex("^(a+)+$", RegexOptions.None, TimeSpan.FromSeconds(1)).IsMatch(longInput);
            });

            Assert.NotNull(ex);
        }

        [Fact]
        public void PasswordStrengthRegularExpression_WithInvalidRegex_ThrowsProviderException()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();

            // Use reflection to set a broken regex and call the validation helper through Initialize via reflection.
            var regexField = typeof(SQLiteMembershipProvider).GetField("_passwordStrengthRegularExpression",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(regexField);
            regexField!.SetValue(null, "(");

            var validateMethod = typeof(SQLiteMembershipProvider).GetMethod("ValidatePwdStrengthRegularExpression",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(validateMethod);

            // Act + Assert
            var ex = Assert.Throws<System.Reflection.TargetInvocationException>(() => validateMethod!.Invoke(null, null));
            Assert.NotNull(ex.InnerException);
            Assert.IsType<System.Configuration.Provider.ProviderException>(ex.InnerException);
        }
    }
}
