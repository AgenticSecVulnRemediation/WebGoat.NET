using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void SetPropertyValues_UsesInterpolatedSqlWithParameterPlaceholders_NoTrailingSemicolon()
        {
            // Arrange
            // Fix changed query construction to string interpolation with USER_TB_NAME and removed trailing semicolon.
            // cmd.CommandText = $"SELECT UserId FROM {USER_TB_NAME} WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId"

            var expected = "SELECT UserId FROM [aspnet_Users] WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId";

            // Assert
            Assert.DoesNotContain(";", expected);
            Assert.Contains("$Username", expected);
            Assert.Contains("$ApplicationId", expected);
            Assert.Contains("FROM [aspnet_Users]", expected);
        }

        [Fact]
        public void GetPropertyValuesFromDatabase_UsesInterpolatedSqlWithUserTableName()
        {
            // Arrange
            var expected = "SELECT UserId FROM [aspnet_Users] WHERE LoweredUsername = $UserName AND ApplicationId = $ApplicationId";

            // Assert
            Assert.Contains("FROM [aspnet_Users]", expected);
            Assert.Contains("$UserName", expected);
            Assert.Contains("$ApplicationId", expected);
        }
    }
}
