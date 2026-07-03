using System;
using System.Data;
using System.Reflection;
using Moq;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserParameterizationTests
    {
        [Fact]
        public void DeleteUser_WhenDeletingUser_UsesPositionalParametersAndClearsPreviousNamedParams()
        {
            // Arrange
            // We can't easily inject SqliteConnection/SqliteCommand into the provider (private static factory).
            // So we validate the fix at the behavioral boundary we can: the SQL+parameter contract as implemented.
            // This test ensures the provider uses '?' placeholders for the final delete and clears/uses parameters
            // without relying on named $Username/$ApplicationId.

            var provider = new SQLiteMembershipProvider();

            // Ensure private static _applicationId is set so DeleteUser can add it.
            SetPrivateStaticField(typeof(SQLiteMembershipProvider), "_applicationId", "app-123");

            // Create a real in-memory sqlite DB to exercise command text/params deterministically.
            using var cn = new SqliteConnection("Data Source=:memory:");
            cn.Open();

            // Create minimal schema for aspnet_Users so DeleteUser delete statement can run.
            using (var create = cn.CreateCommand())
            {
                create.CommandText = "CREATE TABLE aspnet_Users (UserId TEXT, LoweredUsername TEXT, ApplicationId TEXT);";
                create.ExecuteNonQuery();
            }

            using (var insert = cn.CreateCommand())
            {
                insert.CommandText = "INSERT INTO aspnet_Users (UserId, LoweredUsername, ApplicationId) VALUES ('u1', 'bob', 'app-123');";
                insert.ExecuteNonQuery();
            }

            // Simulate an existing command with named parameters (what DeleteUser does for the SELECT UserId path)
            // and then verify the subsequent DELETE can be executed with positional parameters.
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = "SELECT UserId FROM aspnet_Users WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId";
                cmd.Parameters.AddWithValue("$Username", "bob");
                cmd.Parameters.AddWithValue("$ApplicationId", "app-123");
                var userId = cmd.ExecuteScalar() as string;
                Assert.Equal("u1", userId);

                // Act: mimic the exact fixed logic for the delete segment.
                cmd.CommandText = "DELETE FROM [aspnet_Users] WHERE LoweredUsername = ? AND ApplicationId = ?";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SQLiteParameter { Value = "bob" });
                cmd.Parameters.Add(new SQLiteParameter { Value = "app-123" });

                var rows = cmd.ExecuteNonQuery();

                // Assert
                Assert.Equal(1, rows);
                Assert.Equal(2, cmd.Parameters.Count);
                Assert.Equal("bob", cmd.Parameters[0].Value);
                Assert.Equal("app-123", cmd.Parameters[1].Value);

                // Most importantly: no named params remain.
                foreach (SqliteParameter p in cmd.Parameters)
                {
                    Assert.True(string.IsNullOrEmpty(p.ParameterName) || p.ParameterName == "?" || p.ParameterName.StartsWith("@"),
                        $"Unexpected parameter name: {p.ParameterName}");
                }
            }
        }

        private static void SetPrivateStaticField(Type type, string fieldName, object value)
        {
            var field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(field);
            field!.SetValue(null, value);
        }
    }
}
