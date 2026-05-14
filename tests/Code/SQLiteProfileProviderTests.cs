using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    // NOTE: Namespace inferred from source file path "WebGoat/Code/SQLiteProfileProvider.cs".
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UsesAtUserIdParameterMarker()
        {
            // Patch changed parameter marker from "$UserId" to "@UserId" for the profile table lookup.
            const string sql = "SELECT PropertyNames, PropertyValuesString, PropertyValuesBinary FROM [aspnet_Profile] WHERE UserId = @UserId";

            Assert.Contains("@UserId", sql);
            Assert.DoesNotContain("$UserId", sql);
        }
    }
}
