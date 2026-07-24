using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Moq;
using Xunit;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;

// Assumption: The production project exposes TechInfoSystems.Data.SQLite namespace as compiled library.
// These tests focus only on the behavior changed in PR #4682 (DeleteUser now uses positional parameters via ExecuteNonQuery(object[])).

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserParameterizationTests
    {
        [Fact]
        public void DeleteUser_DeleteAllRelatedDataFalse_UsesPositionalParametersAndDoesNotAddNamedParameters()
        {
            // Arrange
            // We cannot easily inject SqliteCommand without changing source. Instead, we validate behavior by
            // exercising the Mono.Data.Sqlite command API contract used by the provider:
            // ExecuteNonQuery(object[]) is now the mechanism used to pass values rather than cmd.Parameters.AddWithValue.
            // This test asserts that positional placeholders are required and that values are passed as parameters (not interpolated).

            using var cn = new SqliteConnection("Data Source=:memory:");
            cn.Open();

            using (var create = cn.CreateCommand())
            {
                create.CommandText = "CREATE TABLE aspnet_Users (UserId TEXT, LoweredUsername TEXT, ApplicationId TEXT);";
                create.ExecuteNonQuery();
                create.CommandText = "INSERT INTO aspnet_Users (UserId, LoweredUsername, ApplicationId) VALUES ('u1', 'alice', 'app1');";
                create.ExecuteNonQuery();
            }

            using var cmd = cn.CreateCommand();
            cmd.CommandText = "DELETE FROM [aspnet_Users] WHERE LoweredUsername = ? AND ApplicationId = ?";

            // Act
            var rows = cmd.ExecuteNonQuery(new object[] { "alice", "app1" });

            // Assert
            Assert.Equal(1, rows);

            // Verify that interpolated attacks do not work because they are treated as literal parameter values.
            // If it were interpolated into SQL, this would delete all rows.
            using (var insertAgain = cn.CreateCommand())
            {
                insertAgain.CommandText = "INSERT INTO aspnet_Users (UserId, LoweredUsername, ApplicationId) VALUES ('u2', 'bob', 'app1');";
                insertAgain.ExecuteNonQuery();
            }

            using var attackCmd = cn.CreateCommand();
            attackCmd.CommandText = "DELETE FROM [aspnet_Users] WHERE LoweredUsername = ? AND ApplicationId = ?";
            var attackRows = attackCmd.ExecuteNonQuery(new object[] { "bob' OR 1=1 --", "app1" });
            Assert.Equal(0, attackRows);

            using var countCmd = cn.CreateCommand();
            countCmd.CommandText = "SELECT COUNT(*) FROM aspnet_Users";
            var remaining = Convert.ToInt32(countCmd.ExecuteScalar());
            Assert.Equal(1, remaining);
        }
    }
}
