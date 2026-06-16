using System;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderDeleteProfileParameterStyleTests
    {
        [Fact]
        public void DeleteProfiles_WithUsernames_UsesAtParametersAndDoesNotThrow()
        {
            // Arrange
            // Regression test for changing $Username/$ApplicationId/$UserId placeholders to @Username/@ApplicationId/@UserId.
            // We validate by running DeleteProfiles against a minimal schema; placeholder mismatch would throw.

            var provider = new SQLiteProfileProvider();
            var connStr = "Data Source=:memory:;Version=3;New=True;";
            SetStatic("_connectionString", connStr);
            SetStatic("_membershipApplicationId", Guid.NewGuid().ToString());

            using (var cn = new Mono.Data.Sqlite.SqliteConnection(connStr))
            {
                cn.Open();
                using var cmd = cn.CreateCommand();
                cmd.CommandText = "CREATE TABLE [aspnet_Users](UserId TEXT, LoweredUsername TEXT, ApplicationId TEXT);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE TABLE [aspnet_Profile](UserId TEXT);";
                cmd.ExecuteNonQuery();

                var userId = Guid.NewGuid().ToString();
                cmd.CommandText = "INSERT INTO [aspnet_Users](UserId, LoweredUsername, ApplicationId) VALUES ($uid,$un,$app);";
                cmd.Parameters.AddWithValue("$uid", userId);
                cmd.Parameters.AddWithValue("$un", "alice");
                cmd.Parameters.AddWithValue("$app", GetStatic("_membershipApplicationId"));
                cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();
                cmd.CommandText = "INSERT INTO [aspnet_Profile](UserId) VALUES ($uid);";
                cmd.Parameters.AddWithValue("$uid", userId);
                cmd.ExecuteNonQuery();
            }

            // Act
            var ex = Record.Exception(() => provider.DeleteProfiles(new[] { "Alice" }));

            // Assert
            Assert.Null(ex);
        }

        private static void SetStatic(string fieldName, object value)
        {
            var f = typeof(SQLiteProfileProvider).GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(f);
            f!.SetValue(null, value);
        }

        private static string GetStatic(string fieldName)
        {
            var f = typeof(SQLiteProfileProvider).GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(f);
            return (string)f!.GetValue(null);
        }
    }
}
