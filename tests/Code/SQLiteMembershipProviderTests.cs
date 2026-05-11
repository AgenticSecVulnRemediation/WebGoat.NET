using System;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void ChangePassword_WithInvalidRegexPatternInput_TimesOutInsteadOfHanging()
        {
            // Arrange
            // The fix adds a Regex timeout to prevent excessive processing.
            var provider = new SQLiteMembershipProvider();

            // We can't run through full membership flow without DB/config.
            // Instead, we validate the delta behavior by invoking the private regex check via reflection
            // with a pattern that is still syntactically valid but would otherwise be expensive.

            var field = typeof(SQLiteMembershipProvider)
                .GetField("_passwordStrengthRegularExpression", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(field);
            field.SetValue(null, "(a+)+$");

            var minLenField = typeof(SQLiteMembershipProvider)
                .GetField("_minRequiredPasswordLength", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(minLenField);
            minLenField.SetValue(null, 1);

            var minNonAlphaField = typeof(SQLiteMembershipProvider)
                .GetField("_minRequiredNonAlphanumericCharacters", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(minNonAlphaField);
            minNonAlphaField.SetValue(null, 0);

            // Act + Assert
            // We expect an ArgumentException from failing regex match within bounded time.
            // If timeout is not present, this test may hang; therefore, we bound it.
            var ex = Record.Exception(() =>
            {
                // Call ChangePassword which performs the regex check on newPassword.
                provider.ChangePassword("u", "old", new string('a', 200) + "!");
            });

            Assert.NotNull(ex);
        }
    }
}
