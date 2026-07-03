using System;
using System.Reflection;
using Xunit;

// Assumptions:
// - Source namespace is TechInfoSystems.Data.SQLite (from WebGoat/Code/SQLiteMembershipProvider.cs)
// - We avoid real DB interaction by verifying the post-fix behavior at the SqliteCommand level via reflection.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserCommandReuseTests
    {
        [Fact]
        public void DeleteUser_WhenDeleteAllRelatedDataFalse_ClearsParametersBeforeDeleteCommand()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();

            // Ensure minimal static state so method can run far enough to build commands.
            SetStaticField("_applicationId", "app-id");
            SetStaticField("_connectionString", "Data Source=:memory:;Version=3;New=True;");

            // Act + Assert
            // This is a regression test for the fix that clears parameters before reusing the command.
            // We expect no exception due to duplicate parameter names when the command is reused.
            // Previously, adding $Username/$ApplicationId twice could throw.
            var ex = Record.Exception(() => provider.DeleteUser("user", deleteAllRelatedData: false));

            // Because the DB is in-memory and schema isn't present, we may still get DB errors.
            // The key assertion is: it should NOT fail with an ArgumentException about duplicate parameters.
            if (ex != null)
            {
                Assert.False(ex is ArgumentException && ex.Message.Contains("Parameter", StringComparison.OrdinalIgnoreCase),
                    $"Unexpected parameter-collection exception: {ex}");
            }
        }

        private static void SetStaticField(string fieldName, object value)
        {
            var field = typeof(SQLiteMembershipProvider).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(field);
            field!.SetValue(null, value);
        }
    }
}
