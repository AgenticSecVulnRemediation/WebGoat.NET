using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSetPropertyValuesParameterStyleTests
    {
        [Fact]
        public void SetPropertyValues_SelectUserId_UsesAtParameters_NotDollarParameters()
        {
            // Delta: command uses @Username/@ApplicationId placeholders.
            const string cmdText = "SELECT UserId FROM [aspnet_Users] WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId;";

            Assert.Contains("@Username", cmdText);
            Assert.Contains("@ApplicationId", cmdText);
            Assert.DoesNotContain("$Username", cmdText);
            Assert.DoesNotContain("$ApplicationId", cmdText);
        }
    }
}
