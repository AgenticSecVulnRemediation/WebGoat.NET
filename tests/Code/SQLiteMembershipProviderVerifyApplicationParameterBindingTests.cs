// Assumptions:
// - Source namespace TechInfoSystems.Data.SQLite matches the file's declared namespace.
// - This delta test focuses on the change in VerifyApplication():
//   cmd.Parameters.AddWithValue("$ApplicationName", ...) and cmd.Parameters.AddWithValue("$Description", ...)
//   instead of missing '$' prefixes which could break parameter binding.

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
    public class SQLiteMembershipProviderVerifyApplicationParameterBindingTests
    {
        [Fact]
        public void Initialize_VerifyApplication_BindsInsertParameters_DoesNotThrow()
        {
            // Arrange
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

                InjectConnectionString("test", connString);

                // Ensure provider static _applicationId is empty so VerifyApplication runs insert.
                var appIdField = typeof(SQLiteMembershipProvider).GetField("_applicationId", BindingFlags.NonPublic | BindingFlags.Static);
                appIdField?.SetValue(null, null);

                // Act
                var ex = Record.Exception(() => provider.Initialize("SQLiteMembershipProvider", config));

                // Assert
                Assert.Null(ex);

                // And a row should exist in aspnet_Applications.
                using (var cn = new SqliteConnection(connString))
                {
                    cn.Open();
                    using (var cmd = cn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT COUNT(*) FROM aspnet_Applications";
                        var count = Convert.ToInt32(cmd.ExecuteScalar());
                        Assert.Equal(1, count);
                    }
                }
            }
            finally
            {
                if (File.Exists(dbFile))
                    File.Delete(dbFile);
            }
        }

        private static void InjectConnectionString(string name, string connectionString)
        {
            try
            {
                var css = new System.Configuration.ConnectionStringSettings(name, connectionString, "Mono.Data.Sqlite");

                var connStrings = System.Configuration.ConfigurationManager.ConnectionStrings;
                var readOnlyProp = connStrings.GetType().GetProperty("IsReadOnly", BindingFlags.NonPublic | BindingFlags.Instance);
                readOnlyProp?.SetValue(connStrings, false);

                connStrings.Add(css);
            }
            catch
            {
                var field = typeof(SQLiteMembershipProvider).GetField("_connectionString", BindingFlags.NonPublic | BindingFlags.Static);
                field?.SetValue(null, connectionString);
            }
        }
    }
}
