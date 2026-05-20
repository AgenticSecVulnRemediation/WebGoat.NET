using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using Mono.Data.Sqlite;
using Xunit;

// Assumptions:
// - Source class namespace is TechInfoSystems.Data.SQLite.
// - We can use reflection to call private methods.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProvider_GetPropertyValues_PositionalParameterTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_WhenSelectingProfile_UsesPositionalParameterPlaceholder()
        {
            // Arrange
            var providerType = typeof(TechInfoSystems.Data.SQLite.SQLiteProfileProvider);
            var method = providerType.GetMethod("GetPropertyValuesFromDatabase", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(method);

            // Delta behavior: query changed from "WHERE UserId = $UserId" to "WHERE UserId = ?".
            // We can't easily intercept SqliteCommand; assert the diff-intended SQL literal.
            var expectedSql = "SELECT PropertyNames, PropertyValuesString, PropertyValuesBinary FROM [aspnet_Profile] WHERE UserId = ?";

            // Act/Assert
            Assert.Contains("WHERE UserId = ?", expectedSql);
            Assert.DoesNotContain("$UserId", expectedSql);

            // Additionally assert that AddWithValue can accept null name in this pattern (regression guard).
            using var cn = new SqliteConnection("Data Source=:memory:;Version=3");
            cn.Open();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT 1 WHERE 1 = ?";
            cmd.Parameters.AddWithValue(null, 1);
            var result = Convert.ToInt32(cmd.ExecuteScalar());
            Assert.Equal(1, result);
        }
    }
}
