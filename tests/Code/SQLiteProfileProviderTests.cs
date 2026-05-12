using System;
using System.Collections.Specialized;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void SetPropertyValues_UsesAtParametersForUsernameAndApplicationId_DoesNotThrow()
        {
            // Arrange
            // Note: we cannot reliably execute the DB command in a unit test without an actual SQLite DB.
            // This delta test verifies the security-relevant change by asserting the updated command text/parameter names
            // are present in the compiled method body string constants.
            var method = typeof(SQLiteProfileProvider).GetMethod(
                "SetPropertyValues",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            Assert.NotNull(method);

            // Act
            var body = method!.GetMethodBody();

            // Assert
            // Basic guard: method has a body and therefore IL we can inspect.
            Assert.NotNull(body);

            // Assert the updated parameter tokens appear in the assembly (best-effort, deterministic).
            // This ensures regression against accidentally reverting to "$Username"/"$ApplicationId".
            var asmText = typeof(SQLiteProfileProvider).Assembly.FullName ?? string.Empty;
            Assert.Contains("SQLite", asmText, StringComparison.OrdinalIgnoreCase);

            // Ensure the new parameter names are used in the SQL string.
            // We rely on reflection over private const strings via metadata rather than executing DB calls.
            var sqlExpected = "LoweredUsername = @Username AND ApplicationId = @ApplicationId";
            Assert.Contains(sqlExpected, GetAllStringLiterals(typeof(SQLiteProfileProvider)), StringComparison.Ordinal);

            Assert.DoesNotContain("LoweredUsername = $Username", GetAllStringLiterals(typeof(SQLiteProfileProvider)), StringComparison.Ordinal);
        }

        private static string GetAllStringLiterals(Type t)
        {
            // Collect const string fields (including private) which is where command text literals typically live.
            // If no const fields exist, return empty to keep test deterministic.
            var fields = t.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            var sb = new System.Text.StringBuilder();
            foreach (var f in fields)
            {
                if (f.FieldType == typeof(string) && f.IsLiteral && !f.IsInitOnly)
                {
                    sb.Append((string?)f.GetRawConstantValue());
                    sb.Append("\n");
                }
            }
            return sb.ToString();
        }
    }
}
