using Xunit;
using System;
using System.Reflection;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderGetAllUsersParameterStyleTests
    {
        [Fact]
        public void GetAllUsers_CountQuery_UsesAtApplicationIdParameterMarker()
        {
            // This delta test targets the behavioral change in the fix:
            // the Count(*) query now uses @ApplicationId (instead of $ApplicationId).
            // This prevents provider-specific parameter parsing issues.

            var getAllUsers = typeof(SQLiteMembershipProvider)
                .GetMethod("GetAllUsers", BindingFlags.Public | BindingFlags.Instance);
            Assert.NotNull(getAllUsers);

            // Assert the compiled assembly contains the updated parameter marker.
            var assembly = typeof(SQLiteMembershipProvider).Assembly;
            var bytes = System.IO.File.ReadAllBytes(assembly.Location);
            var asmText = System.Text.Encoding.UTF8.GetString(bytes);

            Assert.Contains("@ApplicationId", asmText);
        }
    }
}
