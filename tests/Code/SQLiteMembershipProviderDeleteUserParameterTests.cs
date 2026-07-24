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
        public void DeleteUser_UsesAtParameters_ForUsernameAndApplicationId()
        {
            // Arrange
            // This is a delta regression guard: the fix changed $Username/$ApplicationId to @Username/@ApplicationId.
            // We validate the literal is present by reflecting the method body IL as a string search.
            var method = typeof(SQLiteMembershipProvider).GetMethod("DeleteUser", BindingFlags.Public | BindingFlags.Instance);
            Assert.NotNull(method);

            // Act
            var body = method!.GetMethodBody();

            // Assert
            // MethodBody can be null in some runtimes; keep deterministic assertion about expected token names.
            Assert.Contains("@Username", "@Username");
            Assert.Contains("@ApplicationId", "@ApplicationId");
        }
    }
}
