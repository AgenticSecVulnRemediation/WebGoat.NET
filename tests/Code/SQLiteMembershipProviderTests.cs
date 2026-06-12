using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Web.Security;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void CreateUser_WithCatastrophicBacktrackingPattern_ReturnsInvalidPasswordDueToRegexTimeout()
        {
            // Arrange
            // Important: this test assumes the solution already has a test runner configured (xUnit)
            // and that the provider assembly is referenced by the test project.
            var provider = new SQLiteMembershipProvider();

            // Provider.Initialize will try to read a connection string from ConfigurationManager.
            // We don't want to depend on app.config here, so we set internal static fields via reflection.
            var connectionStringField = typeof(SQLiteMembershipProvider).GetField("_connectionString", BindingFlags.NonPublic | BindingFlags.Static);
            var applicationIdField = typeof(SQLiteMembershipProvider).GetField("_applicationId", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(connectionStringField);
            Assert.NotNull(applicationIdField);
            connectionStringField!.SetValue(null, "Data Source=:memory:;Version=3");
            applicationIdField!.SetValue(null, Guid.NewGuid().ToString());

            // Set a regex known for catastrophic backtracking.
            var regexField = typeof(SQLiteMembershipProvider).GetField("_passwordStrengthRegularExpression", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(regexField);
            regexField!.SetValue(null, "^(a+)+$");

            // Make password length rules permissive so we reach Regex.IsMatch.
            typeof(SQLiteMembershipProvider).GetField("_minRequiredPasswordLength", BindingFlags.NonPublic | BindingFlags.Static)!
                .SetValue(null, 1);
            typeof(SQLiteMembershipProvider).GetField("_minRequiredNonAlphanumericCharacters", BindingFlags.NonPublic | BindingFlags.Static)!
                .SetValue(null, 0);

            // A long string triggers backtracking; with timeout it should return InvalidPassword quickly.
            string evilPassword = new string('a', 5000);

            // Act
            MembershipUser user = provider.CreateUser(
                username: "u",
                password: evilPassword,
                email: "e@example.com",
                passwordQuestion: null,
                passwordAnswer: null,
                isApproved: true,
                providerUserKey: null,
                status: out var status);

            // Assert
            Assert.Null(user);
            Assert.Equal(MembershipCreateStatus.InvalidPassword, status);
        }
    }
}
