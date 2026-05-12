using System;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderDeleteRoleTests
    {
        [Fact]
        public void DeleteRole_UsesAtParameter_ForRoleNameInSubquery()
        {
            // Arrange
            var method = typeof(SQLiteRoleProvider).GetMethod(
                "DeleteRole",
                BindingFlags.Instance | BindingFlags.Public);

            Assert.NotNull(method);

            // Act
            var literals = GetAllStringLiterals(typeof(SQLiteRoleProvider));

            // Assert
            // Ensure the updated command uses @RoleName rather than $RoleName in the nested delete.
            Assert.Contains("LoweredRoleName = @RoleName", literals, StringComparison.Ordinal);
            Assert.DoesNotContain("LoweredRoleName = $RoleName", literals, StringComparison.Ordinal);
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
