using System;
using System.Collections.Specialized;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderVerifyApplicationTests
    {
        [Fact]
        public void VerifyApplication_UsesPositionalParameters_ForInsertIntoApplications()
        {
            // Arrange
            var method = typeof(SQLiteProfileProvider).GetMethod(
                "VerifyApplication",
                BindingFlags.NonPublic | BindingFlags.Static);

            Assert.NotNull(method);

            // Act
            // We assert the updated INSERT statement (positional parameters) is present in the type's string constants.
            var literals = GetAllStringLiterals(typeof(SQLiteProfileProvider));

            // Assert
            Assert.Contains("INSERT INTO", literals, StringComparison.Ordinal);
            Assert.Contains("VALUES (?, ?, ?)", literals, StringComparison.Ordinal);
            Assert.DoesNotContain("VALUES ($ApplicationId, $ApplicationName, $Description)", literals, StringComparison.Ordinal);
        }

        private static string GetAllStringLiterals(Type t)
        {
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
