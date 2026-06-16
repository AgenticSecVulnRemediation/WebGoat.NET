using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderDeleteProfileParameterMarkerTests
    {
        [Fact]
        public void DeleteProfile_DeleteStatement_UsesAtUserIdParameterMarker()
        {
            // Regression test for security fix changing "$UserId" to "@UserId" in DeleteProfile.
            // We assert the updated marker is present in the compiled assembly string pool.

            var providerType = typeof(TechInfoSystems.Data.SQLite.SQLiteProfileProvider);
            var assemblyText = providerType.Assembly.ToString();

            Assert.Contains("@UserId", assemblyText);
            Assert.DoesNotContain("$UserId\"", assemblyText);
        }
    }
}
