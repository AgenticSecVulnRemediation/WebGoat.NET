using System;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProvider_DeleteUserTests
    {
        [Fact]
        public void DeleteUser_WhenDeleteAllRelatedDataTrue_ClearsCommandParametersBetweenSelectAndDelete()
        {
            // This is a regression/delta test for PR #627.
            // The patch introduced cmd.Parameters.Clear() before executing the DELETE, preventing parameter reuse/leakage
            // between the SELECT (UserId) and DELETE statements.
            //
            // We can't easily assert internal SqliteCommand parameter collections without a DB, but we can
            // assert the behavior indirectly by running DeleteUser against an in-memory sqlite DB and ensuring
            // it does not throw due to duplicate parameter names.

            // Arrange: create provider instance without calling Initialize (uses static _connectionString/_applicationId)
            var provider = CreateProviderForInMemoryDb(out var connectionString, out var appId);

            using (var cn = new Mono.Data.Sqlite.SqliteConnection(connectionString))
            {
                cn.Open();
                using var cmd = cn.CreateCommand();

                // Minimal schema used by DeleteUser
                cmd.CommandText = @"
CREATE TABLE [aspnet_Users] (UserId TEXT, LoweredUsername TEXT, ApplicationId TEXT);
CREATE TABLE [aspnet_UsersInRoles] (UserId TEXT);
CREATE TABLE [aspnet_Profile] (UserId TEXT);
INSERT INTO [aspnet_Users] (UserId, LoweredUsername, ApplicationId) VALUES ('u1', 'bob', @appId);";
                cmd.Parameters.AddWithValue("@appId", appId);
                cmd.ExecuteNonQuery();
            }

            // Act/Assert: should not throw (previously could throw if parameters duplicated)
            var ex = Record.Exception(() => provider.DeleteUser("Bob", deleteAllRelatedData: true));
            Assert.Null(ex);
        }

        private static SQLiteMembershipProvider CreateProviderForInMemoryDb(out string connectionString, out string appId)
        {
            connectionString = "Data Source=:memory:;Version=3;New=True;";
            appId = Guid.NewGuid().ToString();

            // Create uninitialized instance then set static fields via reflection
            var provider = (SQLiteMembershipProvider)System.Runtime.Serialization.FormatterServices
                .GetUninitializedObject(typeof(SQLiteMembershipProvider));

            var t = typeof(SQLiteMembershipProvider);
            t.GetField("_connectionString", BindingFlags.Static | BindingFlags.NonPublic)!.SetValue(null, connectionString);
            t.GetField("_applicationId", BindingFlags.Static | BindingFlags.NonPublic)!.SetValue(null, appId);

            return provider;
        }
    }
}
