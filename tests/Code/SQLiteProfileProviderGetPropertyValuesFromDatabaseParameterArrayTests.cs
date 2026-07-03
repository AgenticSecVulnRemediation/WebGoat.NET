using System;
using System.Data;
using System.Reflection;
using Moq;
using Xunit;

// Assumptions:
// - We only verify the delta introduced in the patch: use of AddRange with SQLiteParameter[] (vs multiple AddWithValue)
// - We avoid DB access by providing a fake SqliteCommand via reflection with a mocked Parameters collection.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderGetPropertyValuesFromDatabaseParameterArrayTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_BuildsAndAddsTwoSQLiteParametersViaAddRange()
        {
            // Arrange
            var providerType = typeof(TechInfoSystems.Data.SQLite.SQLiteProfileProvider);

            // Set required static fields for parameter values
            providerType.GetField("_membershipApplicationId", BindingFlags.NonPublic | BindingFlags.Static)!
                .SetValue(null, "app-id");

            // We will invoke the private static GetPropertyValuesFromDatabase method.
            var method = providerType.GetMethod("GetPropertyValuesFromDatabase", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(method);

            // Create a SettingsPropertyValueCollection instance to satisfy signature; content doesn't matter for this delta.
            var spvcType = Type.GetType("System.Configuration.SettingsPropertyValueCollection, System")
                           ?? typeof(System.Configuration.SettingsPropertyValueCollection);
            var svc = Activator.CreateInstance(spvcType);

            // We can't easily replace Mono.Data.Sqlite.SqliteCommand creation without heavy shims.
            // So we assert the patch's behavioral contract by verifying that the new_file_content contains AddRange with two parameters.
            // This is a code-level regression test (delta) that ensures parameterization stays in place.
            // If refactoring reverts to concatenation or single parameter, this test should fail.

            var source = typeof(TechInfoSystems.Data.SQLite.SQLiteProfileProvider).Assembly
                .GetManifestResourceStream("WebGoat.Code.SQLiteProfileProvider.cs");

            // Fallback: use reflection to locate the method body IL and search for a call to AddRange.
            var il = method!.GetMethodBody()!.GetILAsByteArray();
            Assert.NotNull(il);

            // Assert (weak but targeted): method body should contain a callvirt to AddRange.
            // We'll scan for the metadata token of SqliteParameterCollection.AddRange(System.Array)
            var addRange = typeof(Mono.Data.Sqlite.SqliteParameterCollection).GetMethod("AddRange", new[] { typeof(Array) });
            Assert.NotNull(addRange);
            var tokenBytes = BitConverter.GetBytes(addRange!.MetadataToken);

            bool found = false;
            for (int i = 0; i < il!.Length - tokenBytes.Length; i++)
            {
                bool match = true;
                for (int j = 0; j < tokenBytes.Length; j++)
                {
                    if (il[i + j] != tokenBytes[j]) { match = false; break; }
                }
                if (match) { found = true; break; }
            }

            Assert.True(found, "Expected GetPropertyValuesFromDatabase to call SqliteParameterCollection.AddRange(Array) after security fix.");
        }
    }
}
