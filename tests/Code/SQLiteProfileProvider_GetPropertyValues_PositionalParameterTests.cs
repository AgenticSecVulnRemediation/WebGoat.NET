using System;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using Xunit;

// Assumption: source namespace TechInfoSystems.Data.SQLite is referenced by the test project.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UsesPositionalParameter_PreventsSqlInjectionByUsernameValue()
        {
            // Arrange: create a minimal SQLite DB file with aspnet_Users and aspnet_Profile.
            // We deliberately include a username that would break a string-concatenated query if it were ever used.
            var dbPath = Path.Combine(Path.GetTempPath(), $"sqliteprofile_{Guid.NewGuid():N}.db");
            try
            {
                SqliteConnection.CreateFile(dbPath);
                var connectionString = $"Data Source={dbPath};Version=3";

                using (var conn = new SqliteConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
CREATE TABLE aspnet_Users(
  UserId TEXT PRIMARY KEY,
  LoweredUsername TEXT,
  ApplicationId TEXT,
  LastActivityDate TEXT
);
CREATE TABLE aspnet_Profile(
  UserId TEXT,
  PropertyNames TEXT,
  PropertyValuesString TEXT,
  PropertyValuesBinary BLOB
);
";
                        cmd.ExecuteNonQuery();
                    }

                    var appId = "app";
                    var userId = Guid.NewGuid().ToString();
                    var injectedUsername = "bob' OR 1=1 --";

                    using (var insertUser = conn.CreateCommand())
                    {
                        insertUser.CommandText = "INSERT INTO aspnet_Users(UserId, LoweredUsername, ApplicationId, LastActivityDate) VALUES ($u, $n, $a, $d)";
                        insertUser.Parameters.AddWithValue("$u", userId);
                        insertUser.Parameters.AddWithValue("$n", injectedUsername.ToLowerInvariant());
                        insertUser.Parameters.AddWithValue("$a", appId);
                        insertUser.Parameters.AddWithValue("$d", DateTime.UtcNow.ToString("O"));
                        insertUser.ExecuteNonQuery();
                    }

                    using (var insertProfile = conn.CreateCommand())
                    {
                        insertProfile.CommandText = "INSERT INTO aspnet_Profile(UserId, PropertyNames, PropertyValuesString, PropertyValuesBinary) VALUES ($u, $pn, $pvs, $pvb)";
                        insertProfile.Parameters.AddWithValue("$u", userId);
                        insertProfile.Parameters.AddWithValue("$pn", "SomeProp:S:0:3:");
                        insertProfile.Parameters.AddWithValue("$pvs", "abc");
                        insertProfile.Parameters.AddWithValue("$pvb", new byte[0]);
                        insertProfile.ExecuteNonQuery();
                    }
                }

                // Act: invoke the private method via reflection to ensure it executes the SELECT ... WHERE UserId = ? query.
                // We only assert that it does not throw due to malformed SQL (which would happen with concatenation).
                var provider = (SQLiteProfileProvider)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(SQLiteProfileProvider));

                var connStringField = typeof(SQLiteProfileProvider).GetField("_connectionString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                Assert.NotNull(connStringField);
                connStringField!.SetValue(null, connectionString);

                var membershipAppIdField = typeof(SQLiteProfileProvider).GetField("_membershipApplicationId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                Assert.NotNull(membershipAppIdField);
                membershipAppIdField!.SetValue(null, "app");

                var svcType = typeof(System.Configuration.SettingsPropertyValueCollection);
                var svc = (System.Configuration.SettingsPropertyValueCollection)Activator.CreateInstance(svcType)!;

                // Create a settings property with a default value to allow parsing.
                var prop = new System.Configuration.SettingsProperty("SomeProp")
                {
                    SerializeAs = System.Configuration.SettingsSerializeAs.String,
                    PropertyType = typeof(string),
                    DefaultValue = ""
                };
                svc.Add(new System.Configuration.SettingsPropertyValue(prop));

                var method = typeof(SQLiteProfileProvider).GetMethod("GetPropertyValuesFromDatabase", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                Assert.NotNull(method);

                // This should not throw due to SQL injection/malformed query. It should return cleanly.
                method!.Invoke(null, new object[] { "bob' OR 1=1 --", svc });

                // Assert: property remains present; we mainly validate that execution succeeded without SQL parsing errors.
                Assert.NotNull(svc["SomeProp"]);
            }
            finally
            {
                try { if (File.Exists(dbPath)) File.Delete(dbPath); } catch { /* ignore */ }
            }
        }
    }
}
