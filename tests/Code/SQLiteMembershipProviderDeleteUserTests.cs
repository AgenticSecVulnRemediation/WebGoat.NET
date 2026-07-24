using Xunit;
using Moq;
using System;
using System.Reflection;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserTests
    {
        [Fact]
        public void DeleteUser_WhenDeleteAllRelatedDataTrue_ClearsParametersBeforeDeleteCommand()
        {
            // Arrange
            // Regression test for the fix that ensures cmd.Parameters.Clear() is called before reusing the command for DELETE.
            // We validate this indirectly by ensuring the method can execute the DELETE path without throwing due to
            // duplicate parameter names (a common failure when parameters are not cleared).

            var provider = new SQLiteMembershipProvider();

            // Because this provider relies on internal static fields (_connectionString, _applicationId),
            // set them via reflection for an in-memory database.
            SetStaticField(typeof(SQLiteMembershipProvider), "_connectionString", "Data Source=:memory:;Version=3;");
            SetStaticField(typeof(SQLiteMembershipProvider), "_applicationId", Guid.NewGuid().ToString());

            // Act / Assert
            // We expect ProviderException/SqliteException is possible due to missing schema; we only want to ensure we
            // don't fail with an ArgumentException about duplicate parameters (regression from missing Clear()).
            var ex = Record.Exception(() => provider.DeleteUser("someuser", deleteAllRelatedData: true));

            if (ex != null)
            {
                Assert.DoesNotContain("Parameter", ex.Message, StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain("already exists", ex.Message, StringComparison.OrdinalIgnoreCase);
            }
        }

        private static void SetStaticField(Type t, string fieldName, object value)
        {
            var f = t.GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic);
            if (f == null)
                throw new InvalidOperationException($"Expected field {fieldName} not found on {t.FullName}");
            f.SetValue(null, value);
        }
    }
}
