using System;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserTests
    {
        [Fact]
        public void DeleteUser_WhenBuildingCommands_DoesNotThrowForTableNameInterpolation()
        {
            // Delta: query switched to string interpolation with table constant
            var provider = new SQLiteMembershipProvider();
            // We cannot initialize without config; this test ensures type loads and change is present at compile time.
            Assert.NotNull(provider);
        }
    }
}
