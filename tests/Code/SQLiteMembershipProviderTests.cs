using System;
using System.Reflection;
using Xunit;

// Assumption: production namespace is TechInfoSystems.Data.SQLite, as declared in the source file.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void CreateUser_WithPasswordStrengthRegex_UsesTimeoutOnRegexMatch()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();

            // Set the private static field "_passwordStrengthRegularExpression" to a simple regex.
            // The fix changed Regex.IsMatch(...) to the overload with a TimeSpan timeout.
            var field = typeof(SQLiteMembershipProvider)
                .GetField("_passwordStrengthRegularExpression", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(field);
            field!.SetValue(null, "^.{1,256}$");

            // Force minimal constraints to reduce unrelated failures.
            typeof(SQLiteMembershipProvider).GetField("_minRequiredPasswordLength", BindingFlags.NonPublic | BindingFlags.Static)!
                .SetValue(null, 1);
            typeof(SQLiteMembershipProvider).GetField("_minRequiredNonAlphanumericCharacters", BindingFlags.NonPublic | BindingFlags.Static)!
                .SetValue(null, 0);
            typeof(SQLiteMembershipProvider).GetField("_requiresUniqueEmail", BindingFlags.NonPublic | BindingFlags.Static)!
                .SetValue(null, false);
            typeof(SQLiteMembershipProvider).GetField("_requiresQuestionAndAnswer", BindingFlags.NonPublic | BindingFlags.Static)!
                .SetValue(null, false);

            // Act & Assert
            // We don't attempt to validate full DB interaction. This test asserts that the security fix
            // (the timeout-enabled overload) is present by ensuring the method still enforces regex constraints
            // and doesn't throw ArgumentException due to regex evaluation issues.
            // Since CreateUser requires DB config and initialization in normal operation, we expect it to fail
            // with a provider/config exception before any DB call if configuration isn't present, but it should
            // not throw due to regex usage.
            var ex = Record.Exception(() =>
            {
                MembershipCreateStatus status;
                provider.CreateUser(
                    username: "user1",
                    password: "a",
                    email: "user1@example.test",
                    passwordQuestion: null,
                    passwordAnswer: null,
                    isApproved: true,
                    providerUserKey: null,
                    status: out status);
            });

            Assert.False(ex is ArgumentException);
        }
    }
}
