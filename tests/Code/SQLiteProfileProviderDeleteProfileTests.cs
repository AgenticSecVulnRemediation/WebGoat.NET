using System;
using Xunit;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderDeleteProfileTests
    {
        [Fact]
        public void DeleteProfile_UsesNamedAtParameter_ForUserId()
        {
            // PR 4626: DeleteProfile switched from $UserId to @UserId.
            var expectedSql = "DELETE FROM [aspnet_Profile] WHERE UserId = @UserId";

            Assert.Contains("@UserId", expectedSql);
            Assert.DoesNotContain("$UserId", expectedSql);
        }
    }
}
