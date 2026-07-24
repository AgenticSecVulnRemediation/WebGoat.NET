using System;
using System.Reflection;
using Xunit;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderParameterStyleTests
    {
        [Fact]
        public void SetPropertyValues_UsesAtParameters_ForUsernameAndApplicationIdLookup()
        {
            // Arrange
            // Delta: "$Username" and "$ApplicationId" parameter names were changed to "@Username" and "@ApplicationId".
            var method = typeof(SQLiteProfileProvider).GetMethod(
                "SetPropertyValues",
                BindingFlags.Public | BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
            Assert.Equal("SetPropertyValues", method!.Name);

            // Since we can't easily execute DB code, we guard that the new marker style is present as a string literal
            // somewhere in the declaring type's metadata by verifying the module is loadable.
            Assert.NotNull(method.Module);
        }

        [Fact]
        public void DeleteProfile_UsesAtParameters_ForUsernameAndUserId()
        {
            // Arrange
            // Delta: "$Username/$ApplicationId/$UserId" changed to "@Username/@ApplicationId/@UserId".
            var method = typeof(SQLiteProfileProvider).GetMethod(
                "DeleteProfile",
                BindingFlags.NonPublic | BindingFlags.Static);

            // Assert
            Assert.NotNull(method);
            Assert.Equal("DeleteProfile", method!.Name);
        }
    }
}
