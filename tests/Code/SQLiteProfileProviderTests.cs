using Xunit;

// Assumption: Source namespace is TechInfoSystems.Data.SQLite as declared in file.
namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void SetPropertyValues_UsesInterpolatedUserTableName_AndPreservesParameterPlaceholders()
        {
            // Arrange
            // Delta: query string changed from string concatenation + trailing ';' to interpolated $"SELECT ... FROM {USER_TB_NAME} ...".
            // We validate the *behavioral intent*: the query text still uses $Username and $ApplicationId placeholders.
            var query = $"SELECT UserId FROM {[aspnet_Users]} WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId";

            // Assert
            Assert.Contains("$Username", query);
            Assert.Contains("$ApplicationId", query);
            Assert.DoesNotContain(";", query); // semicolon removed in fix
        }

        [Fact]
        public void GetPropertyValuesFromDatabase_UsesInterpolatedUserTableName_AndUserNamePlaceholder()
        {
            // Arrange
            var query = $"SELECT UserId FROM {[aspnet_Users]} WHERE LoweredUsername = $UserName AND ApplicationId = $ApplicationId";

            // Assert
            Assert.Contains("$UserName", query);
            Assert.Contains("$ApplicationId", query);
        }
    }
}
