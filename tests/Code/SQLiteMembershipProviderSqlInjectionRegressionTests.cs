using Xunit;

namespace WebGoat.Code.Tests
{
    public class SQLiteMembershipProviderSqlInjectionRegressionTests
    {
        [Fact]
        public void SQLiteMembershipProvider_DeleteUser_ProfileDeletes_UseParameterizedPositionalMarker()
        {
            // Arrange
            // The fix parameterizes the UserId in dependent table deletes using positional markers.
            var source = System.IO.File.ReadAllText("WebGoat/Code/SQLiteMembershipProvider.cs");

            // Act / Assert
            Assert.Contains("DELETE FROM [aspnet_Profile] WHERE UserId = ?", source);
            Assert.Contains("cmd.Parameters.AddWithValue (\"UserId\"", source);
            Assert.DoesNotContain("DELETE FROM [aspnet_Profile] WHERE UserId = $UserId", source);
        }
    }
}
