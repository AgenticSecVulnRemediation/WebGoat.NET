using Xunit;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderParameterPrefixTests
    {
        [Fact]
        public void DeleteProfiles_UsesAtParameters_DoesNotThrowDueToUnboundDollarParameters()
        {
            // Arrange
            // This is a behavioral regression test for the placeholder prefix change.
            // We create an in-memory DB with only the tables/columns needed by DeleteProfile (called by DeleteProfiles).

            var provider = new SQLiteProfileProvider();

            var cs = "Data Source=:memory:;Version=3;New=True;";
            SetStaticField(typeof(SQLiteProfileProvider), "_connectionString", cs);
            SetStaticField(typeof(SQLiteProfileProvider), "_membershipApplicationId", Guid.NewGuid().ToString());

            using var cn = new SqliteConnection(cs);
            cn.Open();
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
CREATE TABLE [aspnet_Users] (UserId TEXT PRIMARY KEY, LoweredUsername TEXT, ApplicationId TEXT);
CREATE TABLE [aspnet_Profile] (UserId TEXT PRIMARY KEY);
";
                cmd.ExecuteNonQuery();
            }

            // Act/Assert
            // If the SQL still used $Username/$ApplicationId but code binds @Username/@ApplicationId, SQLite would fail with
            // "parameter not found". We expect it to simply return 0 deletions with the empty dataset.
            var ex = Record.Exception(() =>
            {
                var deleted = provider.DeleteProfiles(new[] { "user-does-not-exist" });
                Assert.Equal(0, deleted);
            });

            Assert.Null(ex);
        }

        private static void SetStaticField(Type t, string fieldName, object value)
        {
            var f = t.GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic);
            Assert.NotNull(f);
            f!.SetValue(null, value);
        }
    }
}
