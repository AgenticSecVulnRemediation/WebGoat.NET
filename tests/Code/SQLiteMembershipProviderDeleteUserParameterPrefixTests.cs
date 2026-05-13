using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserParameterPrefixTests
    {
        [Fact]
        public void DeleteUser_UsesConsistentAtParameters_ForSqliteCommand()
        {
            // Delta test: PR updates DeleteUser to use @-prefixed parameters for SqliteCommand.
            // This prevents SQL injection and avoids mixing parameter prefix styles.

            var path = System.IO.Path.Combine(System.AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "Code", "SQLiteMembershipProvider.cs");
            if (!System.IO.File.Exists(path))
            {
                return;
            }

            var fileText = System.IO.File.ReadAllText(path);

            // Assert: DeleteUser uses @Username and @ApplicationId in the DELETE statement.
            Assert.Contains("DELETE FROM \" + USER_TB_NAME + \" WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId", fileText);
            Assert.Contains("cmd.Parameters.AddWithValue (\"@Username\"", fileText);
            Assert.Contains("cmd.Parameters.AddWithValue (\"@ApplicationId\"", fileText);

            // Assert: the DELETE statement no longer uses $Username/$ApplicationId placeholders.
            Assert.DoesNotContain("WHERE LoweredUsername = $Username", fileText);
            Assert.DoesNotContain("ApplicationId = $ApplicationId\";\n\n\t\t\t\t\tcmd.Parameters.AddWithValue (\"$Username\"", fileText);
        }
    }
}
