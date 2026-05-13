// Assumptions:
// - Source namespace TechInfoSystems.Data.SQLite matches the file's declared namespace.
// - Project references Mono.Data.Sqlite and System.Web (as required by the provider).
// - This test focuses only on the delta: GetAllUsers now uses "@ApplicationId" parameter marker instead of "$ApplicationId".

using System;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Reflection;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderGetAllUsersParameterMarkerTests
    {
        [Fact]
        public void GetAllUsers_UsesAtApplicationIdParameterMarker_DoesNotThrow()
        {
            // Arrange: create temp sqlite db with minimum tables/rows used by VerifyApplication + GetAllUsers.
            var dbFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".db");
            try
            {
                var connString = $"Data Source={dbFile};Version=3;";

                using (var cn = new SqliteConnection(connString))
                {
                    cn.Open();
                    using (var cmd = cn.CreateCommand())
                    {
                        cmd.CommandText = @"
CREATE TABLE aspnet_Applications (ApplicationId TEXT PRIMARY KEY, ApplicationName TEXT, Description TEXT);
CREATE TABLE [aspnet_Users] (
  UserId TEXT PRIMARY KEY,
  Username TEXT,
  LoweredUsername TEXT,
  ApplicationId TEXT,
  Email TEXT,
  LoweredEmail TEXT,
  Comment TEXT,
  Password TEXT,
  PasswordFormat TEXT,
  PasswordSalt TEXT,
  PasswordQuestion TEXT,
  PasswordAnswer TEXT,
  IsApproved INTEGER,
  IsAnonymous INTEGER,
  LastActivityDate TEXT,
  LastLoginDate TEXT,
  LastPasswordChangedDate TEXT,
  CreateDate TEXT,
  IsLockedOut INTEGER,
  LastLockoutDate TEXT,
  FailedPasswordAttemptCount INTEGER,
  FailedPasswordAttemptWindowStart TEXT,
  FailedPasswordAnswerAttemptCount INTEGER,
  FailedPasswordAnswerAttemptWindowStart TEXT
);
";
                        cmd.ExecuteNonQuery();
                    }
                }

                var provider = new SQLiteMembershipProvider();
                var config = new NameValueCollection
                {
                    { "connectionStringName", "test" },
                    { "applicationName", "/" },
                    { "enablePasswordReset", "true" },
                    { "enablePasswordRetrieval", "false" },
                    { "requiresQuestionAndAnswer", "false" },
                    { "requiresUniqueEmail", "false" },
                    { "maxInvalidPasswordAttempts", "5" },
                    { "passwordAttemptWindow", "10" },
                    { "minRequiredPasswordLength", "7" },
                    { "minRequiredNonalphanumericCharacters", "1" },
                    { "passwordStrengthRegularExpression", "" },
                    { "passwordFormat", "Hashed" },
                };

                // Inject the connection string into ConfigurationManager.ConnectionStrings via reflection.
                // This avoids relying on an external config file.
                InjectConnectionString("test", connString);

                provider.Initialize("SQLiteMembershipProvider", config);

                // Act/Assert: GetAllUsers should execute its Count query with @ApplicationId marker.
                // With the old '$ApplicationId' marker (in this code path), Mono.Data.Sqlite throws because
                // the parameter marker doesn't match the provided parameter name.
                int total;
                var ex = Record.Exception(() => provider.GetAllUsers(0, 10, out total));
                Assert.Null(ex);
            }
            finally
            {
                if (File.Exists(dbFile))
                    File.Delete(dbFile);
            }
        }

        private static void InjectConnectionString(string name, string connectionString)
        {
            // ConfigurationManager.ConnectionStrings is read-only; we modify the underlying settings collection.
            // This is test-only and keeps the test deterministic.
            var settingsType = typeof(System.Configuration.ConfigurationManager)
                .GetField("s_configSystem", BindingFlags.NonPublic | BindingFlags.Static)?.FieldType;

            // If reflection fails in some runtime, fall back to setting the provider's private field directly.
            if (settingsType == null)
            {
                SetProviderConnectionStringFallback(connectionString);
                return;
            }

            try
            {
                var css = new System.Configuration.ConnectionStringSettings(name, connectionString, "Mono.Data.Sqlite");

                var connStrings = System.Configuration.ConfigurationManager.ConnectionStrings;
                var readOnlyProp = connStrings.GetType().GetProperty("IsReadOnly", BindingFlags.NonPublic | BindingFlags.Instance);
                readOnlyProp?.SetValue(connStrings, false);

                // ConnectionStringSettingsCollection has Add
                connStrings.Add(css);
            }
            catch
            {
                SetProviderConnectionStringFallback(connectionString);
            }
        }

        private static void SetProviderConnectionStringFallback(string connectionString)
        {
            // Last-resort: set SQLiteMembershipProvider._connectionString directly.
            var field = typeof(SQLiteMembershipProvider).GetField("_connectionString", BindingFlags.NonPublic | BindingFlags.Static);
            field?.SetValue(null, connectionString);
        }
    }
}
