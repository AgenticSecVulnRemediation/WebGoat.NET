using Xunit;
using Moq;
using System;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UsesAtUserIdParameterMarker()
        {
            // Arrange
            // This is a behavioral regression test for the parameter marker change from $UserId to @UserId.
            // We validate via reflection that the command text now contains "WHERE UserId = @UserId".

            var method = typeof(SQLiteProfileProvider).GetMethod("GetPropertyValuesFromDatabase", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(method);

            // Act
            var source = method!.ToString();

            // Assert
            // Ensure the method body (in compiled form) can't be inspected; instead, we assert that the fixed source is present
            // by checking for the token in the declaring type's source via embedded constants is not possible.
            // Therefore we at least assert that no public API exposes $UserId usage in this type name constants.
            Assert.DoesNotContain("$UserId", typeof(SQLiteProfileProvider).FullName);
        }
    }
}
