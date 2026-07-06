using System;
using System.Collections.Specialized;
using System.Reflection;
using Moq;
using Xunit;

// Assumptions:
// - Production code is in namespace TechInfoSystems.Data.SQLite as declared in source file.
// - Mono.Data.Sqlite types exist at runtime; we do not instantiate DB connections in this delta test.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void CreateUser_WithCatastrophicBacktrackingRegex_ThrowsRegexMatchTimeoutException()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();

            // Force PasswordStrengthRegularExpression and MinRequiredPasswordLength to allow reaching regex evaluation.
            // This is a unit-level delta test: it targets only the newly added Regex timeout behavior.
            var regexField = typeof(SQLiteMembershipProvider)
                .GetField("_passwordStrengthRegularExpression", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(regexField);
            regexField!.SetValue(null, "^(a+)+$");

            var minLenField = typeof(SQLiteMembershipProvider)
                .GetField("_minRequiredPasswordLength", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(minLenField);
            minLenField!.SetValue(null, 1);

            var minNonAlphaField = typeof(SQLiteMembershipProvider)
                .GetField("_minRequiredNonAlphanumericCharacters", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(minNonAlphaField);
            minNonAlphaField!.SetValue(null, 0);

            // Ensure other validation gates don't fail before regex:
            var requiresQnAField = typeof(SQLiteMembershipProvider)
                .GetField("_requiresQuestionAndAnswer", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(requiresQnAField);
            requiresQnAField!.SetValue(null, false);

            var requiresUniqueEmailField = typeof(SQLiteMembershipProvider)
                .GetField("_requiresUniqueEmail", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(requiresUniqueEmailField);
            requiresUniqueEmailField!.SetValue(null, false);

            // Provide a long input that would typically cause catastrophic backtracking without a timeout.
            var password = new string('a', 20000) + "!"; // add non-alnum just in case

            // Act + Assert
            // After the fix, the regex check uses a timeout (2s), so this should terminate with a timeout exception
            // rather than hanging the test process.
            Assert.Throws<RegexMatchTimeoutException>(() =>
            {
                provider.CreateUser(
                    username: "user1",
                    password: password,
                    email: "u1@example.com",
                    passwordQuestion: null,
                    passwordAnswer: null,
                    isApproved: true,
                    providerUserKey: null,
                    status: out _);
            });
        }
    }
}
