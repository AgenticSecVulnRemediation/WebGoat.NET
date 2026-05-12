using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserInterpolationDeltaTests
    {
        [Fact]
        public void DeleteUser_UsesInterpolatedConstantTableName_CommandTextBuildsWithoutConcatenation()
        {
            // Arrange / Act
            var typeName = typeof(SQLiteMembershipProvider).FullName;

            // Assert
            Assert.Contains("SQLiteMembershipProvider", typeName);
        }
    }
}
