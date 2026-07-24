using System;
using System.Reflection;
using Xunit;
using Moq;

// Assumption: production namespace is as declared in source.
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

            // Use reflection to set private static fields required by CreateUser's validation.
            // We avoid hitting DB by making CreateUser fail during regex validation.
            SetPrivateStaticField(typeof(SQLiteMembershipProvider), "_minRequiredPasswordLength", 1);
            SetPrivateStaticField(typeof(SQLiteMembershipProvider), "_minRequiredNonAlphanumericCharacters", 0);
            SetPrivateStaticField(typeof(SQLiteMembershipProvider), "_requiresUniqueEmail", false);
            SetPrivateStaticField(typeof(SQLiteMembershipProvider), "_requiresQuestionAndAnswer", false);
            SetPrivateStaticField(typeof(SQLiteMembershipProvider), "_passwordStrengthRegularExpression", "^(a+)+$"
            );

            // passwordStrengthRegularExpression should be evaluated with a timeout (500ms) now.
            // This payload is designed to trigger catastrophic backtracking.
            var password = new string('a', 5000) + "!";

            // Act + Assert
            // After the fix, Regex.IsMatch should time out rather than running indefinitely.
            Assert.Throws<RegexMatchTimeoutException>(() =>
            {
                provider.CreateUser(
                    username: "user",
                    password: password,
                    email: "u@example.com",
                    passwordQuestion: null,
                    passwordAnswer: null,
                    isApproved: true,
                    providerUserKey: null,
                    status: out _);
            });
        }

        private static void SetPrivateStaticField(Type type, string fieldName, object value)
        {
            var field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
            if (field == null)
                throw new InvalidOperationException($"Field not found: {fieldName}");

            field.SetValue(null, value);
        }
    }
}
