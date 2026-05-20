using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Xunit;

// Assumption: source classes live under namespace TechInfoSystems.Data.SQLite as declared in the patched file.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderCreateUserRegexTimeoutTests
    {
        [Fact]
        public void CreateUser_WithCatastrophicBacktrackingRegex_ThrowsRegexMatchTimeoutException()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();

            SetPrivateStaticField(typeof(SQLiteMembershipProvider), "_passwordStrengthRegularExpression", "^(a+)+$");
            SetPrivateStaticField(typeof(SQLiteMembershipProvider), "_minRequiredPasswordLength", 1);
            SetPrivateStaticField(typeof(SQLiteMembershipProvider), "_minRequiredNonAlphanumericCharacters", 0);

            // CreateUser hits Regex.IsMatch(password, pattern, RegexOptions.None, TimeSpan.FromSeconds(1))
            // before touching the DB, so we can trigger the timeout deterministically.
            var longPassword = new string('a', 50000) + "!";

            // Act + Assert
            Assert.Throws<RegexMatchTimeoutException>(() =>
            {
                provider.CreateUser(
                    username: "user",
                    password: longPassword,
                    email: "user@example.com",
                    passwordQuestion: "q",
                    passwordAnswer: "a",
                    isApproved: true,
                    providerUserKey: null,
                    status: out _);
            });
        }

        private static void SetPrivateStaticField(Type type, string fieldName, object value)
        {
            var field = type.GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic);
            Assert.NotNull(field);
            field!.SetValue(null, value);
        }
    }
}
