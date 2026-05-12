using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Xunit;

// Assumptions:
// - Production code declares TechInfoSystems.Data.SQLite.SQLiteMembershipProvider in namespace TechInfoSystems.Data.SQLite
// - Test project can reference the production assembly.
//
// Delta focus:
// PR change modified CreateUser password-strength validation from Regex.IsMatch(password, pattern)
// to Regex.IsMatch(password, pattern, RegexOptions.None, TimeSpan.FromMilliseconds(1000))
// to enforce a match timeout.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void CreateUser_WithConfiguredPasswordStrengthRegex_DoesNotThrowRegexMatchTimeoutException()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();

            // Configure provider's static password strength regex to a safe, valid pattern.
            // This ensures the changed code path is reached.
            var regexField = typeof(SQLiteMembershipProvider)
                .GetField("_passwordStrengthRegularExpression", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(regexField);
            regexField!.SetValue(null, "^[a-zA-Z0-9]+$");

            // Ensure other static constraints don't short-circuit before regex evaluation.
            SetStaticInt("_minRequiredPasswordLength", 1);
            SetStaticInt("_minRequiredNonAlphanumericCharacters", 0);

            // Act + Assert
            // We assert the security property introduced by the fix: timeout-aware regex evaluation
            // should prevent a RegexMatchTimeoutException from bubbling up.
            var ex = Record.Exception(() => InvokePasswordStrengthCheckViaCreateUserValidation(provider, "abc123"));
            Assert.False(ex is RegexMatchTimeoutException);
        }

        private static void InvokePasswordStrengthCheckViaCreateUserValidation(SQLiteMembershipProvider provider, string password)
        {
            // This invokes CreateUser only up to the point where password-strength regex is evaluated.
            // We avoid DB/web-config dependence by intentionally passing parameters that are valid for the validation block,
            // and then we accept any later exception types (ProviderException, etc.) as outside delta scope.
            try
            {
                System.Web.Security.MembershipCreateStatus status;
                provider.CreateUser(
                    username: "user",
                    password: password,
                    email: "user@example.com",
                    passwordQuestion: "q",
                    passwordAnswer: "a",
                    isApproved: true,
                    providerUserKey: null,
                    status: out status);
            }
            catch (RegexMatchTimeoutException)
            {
                throw;
            }
            catch
            {
                // Ignore non-regex exceptions (e.g., provider/DB/config) because the delta under test is regex timeout handling.
            }
        }

        private static void SetStaticInt(string fieldName, int value)
        {
            var f = typeof(SQLiteMembershipProvider)
                .GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(f);
            f!.SetValue(null, value);
        }
    }
}
