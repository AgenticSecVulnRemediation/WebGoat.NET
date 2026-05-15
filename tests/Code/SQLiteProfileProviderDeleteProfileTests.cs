using Xunit;
using System.IO;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderDeleteProfileTests
    {
        [Fact]
        public void DeleteProfile_UsesAtUserIdParameter_NotDollarUserId()
        {
            // Arrange
            var source = File.ReadAllText("WebGoat/Code/SQLiteProfileProvider.cs");

            // Act & Assert
            Assert.Contains("DELETE FROM \" + PROFILE_TB_NAME + \" WHERE UserId = @UserId", source);
            Assert.Contains("AddWithValue(\"@UserId\"", source);
            Assert.DoesNotContain("WHERE UserId = $UserId", source);
        }
    }
}
