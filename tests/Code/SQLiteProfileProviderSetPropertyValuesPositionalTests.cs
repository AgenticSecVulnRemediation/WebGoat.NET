using System;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSetPropertyValuesPositionalTests
    {
        [Fact]
        public void PositionalParameterQueries_AcceptAddWithValueNamesWithoutBreaking()
        {
            // Arrange
            // Diff changed UPDATE/INSERT to positional placeholders (?,?,...) while still using AddWithValue.
            // The important regression is that executing such a command should not throw due to parameter name mismatch.

            using (var cn = new SqliteConnection("Data Source=:memory:;Version=3"))
            {
                cn.Open();

                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE aspnet_Profile(UserId TEXT PRIMARY KEY, PropertyNames TEXT, PropertyValuesString TEXT, PropertyValuesBinary BLOB, LastUpdatedDate TEXT);";
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO aspnet_Profile(UserId, PropertyNames, PropertyValuesString, PropertyValuesBinary, LastUpdatedDate) VALUES (?, ?, ?, ?, ?)";

                    // Act
                    var ex = Record.Exception(() =>
                    {
                        cmd.Parameters.AddWithValue("$UserId", "u");
                        cmd.Parameters.AddWithValue("$PropertyNames", "n");
                        cmd.Parameters.AddWithValue("$PropertyValuesString", "v");
                        cmd.Parameters.AddWithValue("$PropertyValuesBinary", new byte[] { 1, 2 });
                        cmd.Parameters.AddWithValue("$LastUpdatedDate", DateTime.UtcNow);
                        cmd.ExecuteNonQuery();
                    });

                    // Assert
                    Assert.Null(ex);
                }
            }
        }
    }
}
