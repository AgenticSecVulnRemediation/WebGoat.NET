using System;
using System.Text.RegularExpressions;
using Xunit;

// Assumption: Source namespace is TechInfoSystems.Data.SQLite as declared in the patched file.
// These tests focus on the security fix adding a Regex match timeout to mitigate Regex DoS.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void CreateUser_WithCatastrophicBacktrackingRegex_ThrowsRegexMatchTimeoutException()
        {
            // Arrange
            // Regex that can cause catastrophic backtracking for long input
            var provider = new SQLiteMembershipProvider();

            // Configure the provider to use a dangerous regex. The fix adds a timeout to Regex.IsMatch.
            // We set it via reflection because this field is private static.
            var field = typeof(SQLiteMembershipProvider).GetField("_passwordStrengthRegularExpression",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(field);
            field!.SetValue(null, "^(a+)+$");

            // Long input likely to trigger timeout when evaluated against catastrophic regex
            var password = new string('a', 50000) + "!";

            // Act + Assert
            // With the fix, Regex.IsMatch uses a timeout and should throw RegexMatchTimeoutException.
            Assert.Throws<RegexMatchTimeoutException>(() =>
            {
                System.Web.Security.MembershipCreateStatus status;
                provider.CreateUser(
                    username: "user1",
                    password: password,
                    email: "user1@example.com",
                    passwordQuestion: null,
                    passwordAnswer: null,
                    isApproved: true,
                    providerUserKey: null,
                    status: out status);
            });
        }
    }
}
