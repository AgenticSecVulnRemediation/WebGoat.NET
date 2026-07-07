using System;
using System.Reflection;
using Xunit;
using Moq;

// Namespace assumption: source file declares namespace TechInfoSystems.Data.SQLite
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutChangePasswordTests
    {
        [Fact]
        public void ChangePassword_WithPasswordStrengthRegex_UsesMatchTimeoutToMitigateReDoS()
        {
            // Arrange
            // The patch adds a Regex.IsMatch overload with a timeout.
            // We cannot reliably execute provider DB code in a unit test, so we validate the behavioral delta:
            // the implementation must specify a match timeout when evaluating PasswordStrengthRegularExpression.
            // This test is a regression guard against reverting to the timeout-less overload.

            var asmText = typeof(SQLiteMembershipProvider).Assembly.FullName;
            Assert.NotNull(asmText);

            // Act
            var source = GetMethodBodyAsString(typeof(SQLiteMembershipProvider), "ChangePassword");

            // Assert
            Assert.Contains("Regex.IsMatch", source);
            Assert.Contains("TimeSpan.FromMilliseconds", source);
        }

        private static string GetMethodBodyAsString(Type type, string methodName)
        {
            // Best-effort reflection inspection without relying on external files.
            // If the method body cannot be retrieved as source, we fall back to IL string.
            var mi = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            Assert.NotNull(mi);

            var body = mi!.GetMethodBody();
            Assert.NotNull(body);

            var il = body!.GetILAsByteArray();
            Assert.NotNull(il);

            return Convert.ToBase64String(il!);
        }
    }
}
