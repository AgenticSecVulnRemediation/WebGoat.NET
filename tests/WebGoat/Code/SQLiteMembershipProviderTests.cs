using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void ChangePassword_WithPasswordStrengthRegex_DoesNotHang()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();

            // Configure a deliberately ambiguous-but-safe regex that could be expensive without a timeout.
            // We do NOT assert on regex semantics; only that the operation is bounded and fails fast.
            var config = new NameValueCollection
            {
                { "connectionStringName", "Default" },
                { "passwordStrengthRegularExpression", "^(a+)+$" },
                { "minRequiredPasswordLength", "7" },
                { "minRequiredNonalphanumericCharacters", "0" },
                { "enablePasswordReset", "true" },
                { "enablePasswordRetrieval", "false" },
                { "requiresQuestionAndAnswer", "false" },
                { "requiresUniqueEmail", "false" }
            };

            // Provide a dummy connection string entry via ConfigurationManager is not feasible in a pure unit test
            // without app.config; so we bypass Initialize and set the regex field directly.
            SetPrivateStaticField(typeof(SQLiteMembershipProvider), "_passwordStrengthRegularExpression", "^(a+)+$");

            // Act + Assert
            // Expect an ArgumentException (regex mismatch) OR a TimeoutException wrapped in ArgumentException,
            // but most importantly: it must complete quickly and not hang.
            var start = DateTime.UtcNow;
            Assert.ThrowsAny<Exception>(() =>
            {
                // Use reflection to invoke ChangePassword without requiring DB setup.
                // We expect parameter checks to run and then regex evaluation to run, bounded by timeout.
                provider.ChangePassword("user", "old", "aaaaaaaaaaaaaaaaaaaaaaaaaaaa!");
            });

            var elapsed = DateTime.UtcNow - start;
            Assert.True(elapsed < TimeSpan.FromSeconds(2), "ChangePassword should complete quickly due to regex timeout.");
        }

        [Fact]
        public void CreateUser_WithPasswordStrengthRegex_DoesNotHang()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();
            SetPrivateStaticField(typeof(SQLiteMembershipProvider), "_passwordStrengthRegularExpression", "^(a+)+$");

            // Act
            var start = DateTime.UtcNow;
            MembershipCreateStatus status;
            Assert.ThrowsAny<Exception>(() =>
            {
                provider.CreateUser(
                    username: "user",
                    password: "aaaaaaaaaaaaaaaaaaaaaaaaaaaa!",
                    email: "u@example.test",
                    passwordQuestion: null,
                    passwordAnswer: null,
                    isApproved: true,
                    providerUserKey: null,
                    status: out status);
            });

            // Assert
            var elapsed = DateTime.UtcNow - start;
            Assert.True(elapsed < TimeSpan.FromSeconds(2), "CreateUser should complete quickly due to regex timeout.");
        }

        private static void SetPrivateStaticField(Type type, string fieldName, object value)
        {
            var field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
            if (field == null)
            {
                throw new InvalidOperationException($"Field '{fieldName}' not found on type '{type.FullName}'.");
            }
            field.SetValue(null, value);
        }
    }
}
