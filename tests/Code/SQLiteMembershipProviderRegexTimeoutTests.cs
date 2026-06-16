using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.Web.Security;
using Moq;
using Xunit;

// Assumption: production namespace follows file content: TechInfoSystems.Data.SQLite
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutTests
    {
        [Fact]
        public void CreateUser_WithCatastrophicBacktrackingRegex_DoesNotHangAndReturnsInvalidPassword()
        {
            // Arrange
            // The fix adds a Regex timeout in CreateUser when evaluating PasswordStrengthRegularExpression.
            // Use a known catastrophic-backtracking pattern and a long input that would previously take extremely long.
            var provider = new SQLiteMembershipProvider();

            var config = new NameValueCollection
            {
                { "connectionStringName", "Dummy" },
                { "applicationName", "/" },
                { "passwordStrengthRegularExpression", "^(a+)+$" },
                { "minRequiredPasswordLength", "7" },
                { "minRequiredNonalphanumericCharacters", "0" },
                { "requiresUniqueEmail", "false" },
                { "requiresQuestionAndAnswer", "false" }
            };

            // ConfigurationManager.ConnectionStrings is read-only; instead we bypass Initialize and set fields via reflection.
            // This is acceptable for a delta test focused only on the regex-timeout behavior.
            SetPrivateStaticField(typeof(SQLiteMembershipProvider), "_passwordStrengthRegularExpression", "^(a+)+$");
            SetPrivateStaticField(typeof(SQLiteMembershipProvider), "_minRequiredPasswordLength", 1);
            SetPrivateStaticField(typeof(SQLiteMembershipProvider), "_minRequiredNonAlphanumericCharacters", 0);
            SetPrivateStaticField(typeof(SQLiteMembershipProvider), "_requiresUniqueEmail", false);
            SetPrivateStaticField(typeof(SQLiteMembershipProvider), "_requiresQuestionAndAnswer", false);

            // Craft a password that will cause catastrophic backtracking for ^(a+)+$ due to the trailing '!'
            string evilPassword = new string('a', 50000) + "!";

            // Act
            var start = DateTime.UtcNow;
            MembershipCreateStatus status;
            var user = provider.CreateUser(
                username: "user1",
                password: evilPassword,
                email: "u1@example.com",
                passwordQuestion: null,
                passwordAnswer: null,
                isApproved: true,
                providerUserKey: null,
                status: out status);
            var elapsed = DateTime.UtcNow - start;

            // Assert
            // The key security assertion: call returns promptly (timeout should cut off regex evaluation).
            Assert.True(elapsed < TimeSpan.FromSeconds(2), $"CreateUser took too long: {elapsed}");

            // When regex evaluation fails (including due to timeout), provider returns InvalidPassword and null user.
            Assert.Null(user);
            Assert.Equal(MembershipCreateStatus.InvalidPassword, status);
        }

        private static void SetPrivateStaticField(Type type, string fieldName, object value)
        {
            var field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(field);
            field!.SetValue(null, value);
        }
    }
}
