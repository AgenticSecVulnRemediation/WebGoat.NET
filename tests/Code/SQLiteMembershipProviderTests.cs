using System;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void CreateUser_WithCatastrophicBacktrackingPattern_DoesNotHangAndReturnsInvalidPassword()
        {
            // Arrange: use an in-memory sqlite db; the method should fail early due to regex timeout
            var cs = "Data Source=:memory:;Version=3";

            // Minimal provider config. We expect early return InvalidPassword due to regex timeout,
            // so DB schema is not required for this delta test.
            var config = new System.Collections.Specialized.NameValueCollection
            {
                { "connectionStringName", "TestSqlite" },
                { "applicationName", "/" },
                { "passwordStrengthRegularExpression", "^(a+)+$" },
                { "minRequiredPasswordLength", "1" },
                { "minRequiredNonalphanumericCharacters", "0" },
                { "requiresQuestionAndAnswer", "false" },
                { "requiresUniqueEmail", "false" }
            };

            // Inject connection string via ConfigurationManager is difficult in unit tests without app.config.
            // Therefore, directly set the private static _connectionString using reflection.
            var provider = new SQLiteMembershipProvider();
            provider.Initialize("SQLiteMembershipProvider", config);

            var csField = typeof(SQLiteMembershipProvider).GetField("_connectionString", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(csField);
            csField!.SetValue(null, cs);

            // Ensure application id is set to bypass VerifyApplication DB work in this delta test.
            var appIdField = typeof(SQLiteMembershipProvider).GetField("_applicationId", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(appIdField);
            appIdField!.SetValue(null, Guid.NewGuid().ToString());

            // A long string triggers backtracking; with timeout it should return InvalidPassword quickly.
            string evilPassword = new string('a', 5000) + "!";

            // Act
            var user = provider.CreateUser(
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
            Assert.Equal(System.Web.Security.MembershipCreateStatus.InvalidPassword, status);
        }
    }
}
