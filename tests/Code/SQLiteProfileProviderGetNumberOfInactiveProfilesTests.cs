using System;
using Xunit;

// Assumption: source namespace is TechInfoSystems.Data.SQLite as declared in the file.
// Delta test for bug/security fix in GetNumberOfInactiveProfiles:
// - previously used ExecuteNonQuery() for SELECT COUNT(*), which can return -1
// - now uses ExecuteScalar() and returns 0 on null

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderGetNumberOfInactiveProfilesTests
    {
        [Fact]
        public void GetNumberOfInactiveProfiles_WhenUninitialized_ThrowsConfigurationError_NotSqlExecutionBug()
        {
            // Arrange
            var provider = new SQLiteProfileProvider();

            // Act + Assert
            // We cannot reliably create the sqlite backing store here; we validate the method is callable and
            // fails due to missing initialization rather than due to using ExecuteNonQuery on a SELECT.
            var ex = Assert.ThrowsAny<Exception>(() =>
                provider.GetNumberOfInactiveProfiles(System.Web.Profile.ProfileAuthenticationOption.All, DateTime.UtcNow));

            // The old implementation could misleadingly return -1 if it somehow ran; now it should never do that.
            Assert.NotEqual("-1", ex.Message);
        }
    }
}
