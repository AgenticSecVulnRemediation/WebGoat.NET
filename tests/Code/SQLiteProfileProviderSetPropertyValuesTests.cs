using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Web.Profile;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSetPropertyValuesTests
    {
        [Fact]
        public void SetPropertyValues_UsesAtPrefixedParameters_ForUsernameAndApplicationId_WhenResolvingUserId()
        {
            // Arrange
            // Behavioral regression test for parameter marker fix:
            // "$Username/$ApplicationId" -> "@Username/@ApplicationId".
            // We run against an in-memory SQLite DB and verify that the provider adds the expected
            // parameter names to the command used to resolve UserId.

            // Create in-memory DB with minimal schema.
            using var cn = new SqliteConnection("Data Source=:memory:;Version=3;New=True;");
            cn.Open();
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE aspnet_Users (UserId TEXT, LoweredUsername TEXT, ApplicationId TEXT);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO aspnet_Users (UserId, LoweredUsername, ApplicationId) VALUES ('u1', 'bob', 'app1');";
                cmd.ExecuteNonQuery();
            }

            var provider = new SQLiteProfileProvider();
            ForceProviderConnectionString(provider, cn.ConnectionString);
            ForceMembershipApplicationId(provider, "app1");

            // Prepare a minimal SettingsContext + one dirty property to force DB path.
            var sc = new SettingsContext();
            sc.Add("UserName", "Bob");
            sc.Add("IsAuthenticated", true);

            var props = new SettingsPropertyCollection();
            var p = new SettingsProperty("Nick")
            {
                PropertyType = typeof(string),
                Provider = provider,
                SerializeAs = SettingsSerializeAs.String
            };
            p.Attributes["AllowAnonymous"] = false;
            props.Add(p);

            var values = new SettingsPropertyValueCollection();
            var pv = new SettingsPropertyValue(p)
            {
                PropertyValue = "x",
                IsDirty = true
            };
            values.Add(pv);

            // Intercept command parameter names by wrapping SqliteCommand via reflection seam:
            // We cannot inject a factory, so we validate indirectly by asserting the query succeeds
            // against sqlite only when @-parameters are used.
            // If the old $Username/$ApplicationId are used here, the command would not bind correctly
            // for the query that uses @Username/@ApplicationId and would fail to find the row.

            // Act
            provider.SetPropertyValues(sc, values);

            // Assert
            // Verify side-effect: profile row should exist for user 'u1' (meaning the userId resolution succeeded).
            using var verify = cn.CreateCommand();
            verify.CommandText = "SELECT COUNT(*) FROM aspnet_Profile WHERE UserId = 'u1';";
            var count = Convert.ToInt32(verify.ExecuteScalar());
            Assert.Equal(1, count);
        }

        private static void ForceProviderConnectionString(SQLiteProfileProvider provider, string cs)
        {
            var field = typeof(SQLiteProfileProvider).GetField("_connectionString", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(field);
            field!.SetValue(null, cs);
        }

        private static void ForceMembershipApplicationId(SQLiteProfileProvider provider, string appId)
        {
            // Set membership application id directly to avoid relying on aspnet_Applications table.
            var field = typeof(SQLiteProfileProvider).GetField("_membershipApplicationId", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(field);
            field!.SetValue(null, appId);
        }
    }
}
