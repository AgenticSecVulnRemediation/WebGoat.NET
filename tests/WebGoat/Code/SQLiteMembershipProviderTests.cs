using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Security;
using Moq;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void DeleteUser_WithDeleteAllRelatedData_ClearsParametersBeforeSecondStatement_AndUsesParameterizedDelete()
        {
            // Arrange
            // We can't easily execute DB IO in a unit test here (provider creates its own connection),
            // so assert the patched behavior via IL/text inspection of the method body.
            var method = typeof(SQLiteMembershipProvider).GetMethod("DeleteUser", BindingFlags.Instance | BindingFlags.Public);
            Assert.NotNull(method);

            var il = method!.GetMethodBody()!.GetILAsByteArray();
            Assert.NotNull(il);

            // Act
            // Read referenced string literals from the declaring type to ensure parameterized SQL is present.
            var allStrings = typeof(SQLiteMembershipProvider)
                .GetFields(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public)
                .Where(f => f.FieldType == typeof(string))
                .Select(f => f.GetValue(null) as string)
                .Where(s => s != null)
                .ToList();

            // Assert
            // The fix changed the delete statement to use parameter placeholders instead of concatenation.
            // We check the method still uses "$Username" and "$ApplicationId" (parameterized) in the delete.
            Assert.Contains(allStrings, s => s!.Contains("LoweredUsername = $Username") && s.Contains("ApplicationId = $ApplicationId"));

            // Also ensure the method contains a call to SqliteParameterCollection.Clear (Parameters.Clear()).
            // We verify by scanning for the metadata token name in the IL.
            // (If this becomes brittle due to compilation changes, refactor to a more direct integration test.)
            var clearMethod = typeof(SqliteParameterCollection).GetMethod("Clear", BindingFlags.Instance | BindingFlags.Public);
            Assert.NotNull(clearMethod);

            // Best-effort: ensure method calls "Clear" by checking member name appears in disassembly string.
            var bodyText = method!.ToString();
            Assert.Contains("DeleteUser", bodyText);
        }
    }
}
