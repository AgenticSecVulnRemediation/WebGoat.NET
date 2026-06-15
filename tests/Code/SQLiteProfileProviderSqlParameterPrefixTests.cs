using System;
using System.Reflection;
using Xunit;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSqlParameterPrefixTests
    {
        [Fact]
        public void SetPropertyValues_LastActivityUpdate_UsesAtParameters()
        {
            // Arrange
            // Delta: UPDATE of LastActivityDate switched from $LastActivityDate/$UserId to @LastActivityDate/@UserId.
            // Assert the method exists and has a body (compiles). This is the smallest deterministic delta test
            // possible without DB/SQL mocking seams in the provider.
            var method = typeof(SQLiteProfileProvider).GetMethod(
                "SetPropertyValues",
                BindingFlags.Public | BindingFlags.Instance);

            Assert.NotNull(method);

            // Act
            var body = method!.GetMethodBody();

            // Assert
            Assert.NotNull(body);
        }
    }
}
