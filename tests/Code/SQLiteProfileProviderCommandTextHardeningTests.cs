using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderCommandTextHardeningTests
    {
        [Fact]
        public void SetPropertyValues_UsesParameterizedUserLookup_WithAtPrefix()
        {
            var src = System.IO.File.ReadAllText("WebGoat/Code/SQLiteProfileProvider.cs");

            // Delta: replaced string concatenation/$-parameters with interpolated table name and @-parameters
            Assert.Contains("SELECT UserId FROM {USER_TB_NAME} WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId", src);
            Assert.Contains("AddWithValue (\"@Username\"", src);
            Assert.Contains("AddWithValue (\"@ApplicationId\"", src);
            Assert.DoesNotContain("LoweredUsername = $Username", src);
            Assert.DoesNotContain("ApplicationId = $ApplicationId;", src);
        }

        [Fact]
        public void DeleteProfile_UsesParameterizedDelete_WithAtUserId()
        {
            var src = System.IO.File.ReadAllText("WebGoat/Code/SQLiteProfileProvider.cs");

            Assert.Contains("DELETE FROM {PROFILE_TB_NAME} WHERE UserId = @UserId", src);
            Assert.Contains("AddWithValue(\"@UserId\"", src);
            Assert.DoesNotContain("WHERE UserId = $UserId", src);
        }
    }
}
