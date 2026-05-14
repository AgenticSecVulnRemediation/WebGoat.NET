// Assumptions:
// - Source namespace is TechInfoSystems.Data.SQLite (from SQLiteProfileProvider.cs)
// - Mono.Data.Sqlite is available as in the project.
// - This test focuses ONLY on the behavior changed in the diff: parameter markers in DeleteProfile

using System;
using System.Data;
using System.Reflection;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderDeleteProfileParameterMarkerTests
    {
        [Fact]
        public void DeleteProfiles_WhenUserExists_DeletesProfileUsingAtParameters()
        {
            // Arrange: set static fields via reflection
            var providerType = typeof(SQLiteProfileProvider);

            SetStaticField(providerType, "_connectionString", "Data Source=:memory:;Version=3;New=True;");
            SetStaticField(providerType, "_membershipApplicationId", "app-1");

            using var cn = new SqliteConnection("Data Source=:memory:;Version=3;New=True;");
            cn.Open();

            // Create minimal schema for aspnet_Users and aspnet_Profile
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
CREATE TABLE aspnet_Users (
    UserId TEXT PRIMARY KEY,
    LoweredUsername TEXT,
    ApplicationId TEXT
);
CREATE TABLE aspnet_Profile (
    UserId TEXT,
    PropertyNames TEXT,
    PropertyValuesString TEXT,
    PropertyValuesBinary BLOB,
    LastUpdatedDate TEXT
);
";
                cmd.ExecuteNonQuery();
            }

            var userId = Guid.NewGuid().ToString();
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO aspnet_Users(UserId, LoweredUsername, ApplicationId) VALUES (@uid, @uname, @app)";
                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.Parameters.AddWithValue("@uname", "bob");
                cmd.Parameters.AddWithValue("@app", "app-1");
                cmd.ExecuteNonQuery();
            }
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO aspnet_Profile(UserId, PropertyNames, PropertyValuesString, PropertyValuesBinary, LastUpdatedDate) VALUES (@uid, '', '', X'', '2020-01-01')";
                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.ExecuteNonQuery();
            }

            // Insert a transaction into HttpContext is hard in a unit test; instead, directly invoke private DeleteProfile.
            var deleteProfile = providerType.GetMethod("DeleteProfile", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(deleteProfile);

            // Act: username case should be normalized and query should use @Username/@ApplicationId
            var deleted = (bool)deleteProfile!.Invoke(null, new object[] { cn, null!, "Bob" });

            // Assert
            Assert.True(deleted);

            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(*) FROM aspnet_Profile WHERE UserId=@uid";
                cmd.Parameters.AddWithValue("@uid", userId);
                var remaining = Convert.ToInt32(cmd.ExecuteScalar());
                Assert.Equal(0, remaining);
            }
        }

        private static void SetStaticField(Type t, string fieldName, object? value)
        {
            var f = t.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(f);
            f!.SetValue(null, value);
        }
    }
}
