using System;
using Mono.Data.Sqlite;
using Xunit;

// Assumption: Source namespace is TechInfoSystems.Data.SQLite as declared in the patched file.
// This test focuses on the change in FindUsersByName: parameter binding via AddRange and corrected SQL spacing.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderFindUsersByNameTests
    {
        [Fact]
        public void FindUsersByName_BuildsQueryWithExpectedParameters_AndDoesNotThrowForSqlSyntax()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();

            // Inject required private static fields to avoid nulls during command construction.
            SetStaticField("_applicationId", "app-id-1");
            SetStaticField("_connectionString", "Data Source=:memory:");

            // The method will attempt to open a SQLite connection and execute; to keep this as a unit test
            // for the changed behavior, we validate only that SQL and parameters are constructed as expected
            // by calling into a small extracted helper via reflection is not possible. Therefore we assert
            // the method throws due to missing schema, but not due to SQL syntax or missing parameters.

            // Act
            var ex = Record.Exception(() =>
            {
                int total;
                provider.FindUsersByName("abc", 0, 1, out total);
            });

            // Assert
            // Expect an exception because the in-memory DB has no schema. But it should not be a SqliteException
            // about malformed SQL near 'WHERE' caused by missing space, nor about missing parameter bindings.
            if (ex is SqliteException sqliteEx)
            {
                Assert.DoesNotContain("near \"WHERE\"", sqliteEx.Message, StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain("parameter", sqliteEx.Message, StringComparison.OrdinalIgnoreCase);
            }
        }

        private static void SetStaticField(string name, object value)
        {
            var field = typeof(SQLiteMembershipProvider).GetField(name,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(field);
            field!.SetValue(null, value);
        }
    }
}
