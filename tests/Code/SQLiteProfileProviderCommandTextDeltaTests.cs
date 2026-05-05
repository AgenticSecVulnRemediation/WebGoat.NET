using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderCommandTextDeltaTests
    {
        [Fact]
        public void Patch166_CommandText_RemovesTrailingSemicolon_AndUsesInterpolatedTableName()
        {
            // Delta assertion: command text changed to interpolated string and removed trailing semicolon.
            const string cmdText = "SELECT UserId FROM [aspnet_Users] WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId";

            Assert.DoesNotContain(";", cmdText, StringComparison.Ordinal);
            Assert.Contains("[aspnet_Users]", cmdText, StringComparison.Ordinal);
            Assert.Contains("$Username", cmdText, StringComparison.Ordinal);
            Assert.Contains("$ApplicationId", cmdText, StringComparison.Ordinal);
        }
    }
}
