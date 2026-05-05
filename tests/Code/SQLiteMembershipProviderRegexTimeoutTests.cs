using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutTests
    {
        [Fact]
        public void ChangePassword_WithCatastrophicBacktrackingCandidate_CompletesQuickly()
        {
            // Arrange
            // Set up a minimal sqlite membership DB and provider config so ChangePassword reaches the regex check.
            string dbPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".db");
            try
            {
                SetupMembershipSchema(dbPath);

                var provider = new SQLiteMembershipProvider();
                var nvc = new NameValueCollection
                {
                    { "connectionStringName", "TestSqliteMembership" },
                    { "applicationName", "/" },
                    { "minRequiredPasswordLength", "1" },
                    { "minRequiredNonalphanumericCharacters", "0" },
                    // Classic catastrophic backtracking pattern
                    { "passwordStrengthRegularExpression", "^(a+)+$" },
                    { "enablePasswordReset", "true" },
                    { "enablePasswordRetrieval", "false" },
                    { "requiresQuestionAndAnswer", "false" },
                    { "requiresUniqueEmail", "false" },
                    { "passwordFormat", "Clear" }
                };

                InjectConnectionString("TestSqliteMembership", $"Data Source={dbPath};Version=3");
                provider.Initialize("SQLiteMembershipProvider", nvc);

                // Seed user directly (Clear format for simplicity)
                SeedUser(dbPath, appName: "/", username: "u", password: "old", email: "e@e.com");

                // Candidate input that would be slow without a timeout
                string longInput = new string('a', 5000) + "!"; // fails the regex, but forces backtracking attempt

                // Act + Assert
                // With the fix, Regex.IsMatch uses a 500ms timeout; it should throw RegexMatchTimeoutException
                // or complete quickly with an ArgumentException.
                var ex = Record.Exception(() => provider.ChangePassword("u", "old", longInput));

                Assert.NotNull(ex);
                Assert.True(ex is RegexMatchTimeoutException || ex is ArgumentException);
            }
            finally
            {
                try { if (File.Exists(dbPath)) File.Delete(dbPath); } catch { /* ignore */ }
            }
        }

        private static void SetupMembershipSchema(string dbPath)
        {
            using var conn = new SqliteConnection($"Data Source={dbPath};Version=3");
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
CREATE TABLE aspnet_Applications (
  ApplicationId TEXT PRIMARY KEY,
  ApplicationName TEXT,
  Description TEXT
);
CREATE TABLE aspnet_Users (
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

        private static void SeedUser(string dbPath, string appName, string username, string password, string email)
        {
            using var conn = new SqliteConnection($"Data Source={dbPath};Version=3");
            conn.Open();

            string appId = Guid.NewGuid().ToString();
            string userId = Guid.NewGuid().ToString();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO aspnet_Applications(ApplicationId, ApplicationName, Description) VALUES ($id, $name, $desc)";
                cmd.Parameters.AddWithValue("$id", appId);
                cmd.Parameters.AddWithValue("$name", appName);
                cmd.Parameters.AddWithValue("$desc", "");
                cmd.ExecuteNonQuery();
            }

            var now = DateTime.UtcNow.ToString("o");
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
INSERT INTO aspnet_Users(
  UserId, Username, LoweredUsername, ApplicationId, Email, LoweredEmail, Comment,
  Password, PasswordFormat, PasswordSalt, PasswordQuestion, PasswordAnswer,
  IsApproved, IsAnonymous,
  LastActivityDate, LastLoginDate, LastPasswordChangedDate, CreateDate,
  IsLockedOut, LastLockoutDate,
  FailedPasswordAttemptCount, FailedPasswordAttemptWindowStart,
  FailedPasswordAnswerAttemptCount, FailedPasswordAnswerAttemptWindowStart
) VALUES (
  $UserId, $Username, $LoweredUsername, $ApplicationId, $Email, $LoweredEmail, $Comment,
  $Password, $PasswordFormat, $PasswordSalt, $PasswordQuestion, $PasswordAnswer,
  1, 0,
  $Now, $Now, $Now, $Now,
  0, $Now,
  0, $Now,
  0, $Now
);
";
                cmd.Parameters.AddWithValue("$UserId", userId);
                cmd.Parameters.AddWithValue("$Username", username);
                cmd.Parameters.AddWithValue("$LoweredUsername", username.ToLowerInvariant());
                cmd.Parameters.AddWithValue("$ApplicationId", appId);
                cmd.Parameters.AddWithValue("$Email", email);
                cmd.Parameters.AddWithValue("$LoweredEmail", email.ToLowerInvariant());
                cmd.Parameters.AddWithValue("$Comment", "");
                cmd.Parameters.AddWithValue("$Password", password);
                cmd.Parameters.AddWithValue("$PasswordFormat", "Clear");
                cmd.Parameters.AddWithValue("$PasswordSalt", Convert.ToBase64String(new byte[16]));
                cmd.Parameters.AddWithValue("$PasswordQuestion", "");
                cmd.Parameters.AddWithValue("$PasswordAnswer", "");
                cmd.Parameters.AddWithValue("$Now", now);
                cmd.ExecuteNonQuery();
            }
        }

        private static void InjectConnectionString(string name, string cs)
        {
            // Best-effort injection into ConfigurationManager.ConnectionStrings for test runtime.
            // Works on .NET Framework via reflection on the internal collection.
            var settings = new ConnectionStringSettings(name, cs);
            var collection = ConfigurationManager.ConnectionStrings;

            var readOnlyField = typeof(ConfigurationElementCollection).GetField("bReadOnly", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            readOnlyField?.SetValue(collection, false);

            if (collection[name] != null)
            {
                var remove = collection.GetType().GetMethod("Remove", new[] { typeof(string) });
                remove?.Invoke(collection, new object[] { name });
            }

            var add = collection.GetType().GetMethod("Add", new[] { typeof(ConnectionStringSettings) });
            add?.Invoke(collection, new object[] { settings });
        }
    }
}
