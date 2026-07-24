using Xunit;

namespace WebGoat.Code.Tests
{
    public class SQLiteMembershipProviderSqlInjectionRegressionTests
    {
        [Fact]
        public void SQLiteMembershipProvider_DeleteUser_UsesParameterizedQueryMarkersForUserControlledValues()
        {
            // Arrange
            // The vulnerability fix changed DeleteUser to use parameter placeholders (@Username/@ApplicationId)
            // rather than concatenating raw values into the SQL.
            var source = System.IO.File.ReadAllText("WebGoat/Code/SQLiteMembershipProvider.cs");

            // Act / Assert
            Assert.Contains("WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId", source);
            Assert.DoesNotContain("WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId\";\n\n\t\t\t\t\tcmd.Parameters.AddWithValue (\"$Username\"", source);
        }
    }
}
