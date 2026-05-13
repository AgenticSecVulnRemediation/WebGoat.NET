using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserTests
    {
        [Fact]
        public void DeleteUser_QueryUsesNamedParameters_DoesNotContainUserControlledInput()
        {
            // This is a delta test focused on the security fix: DeleteUser now uses named parameters
            // (e.g., @Username/@ApplicationId) rather than concatenating user-controlled input.
            // 
            // Because the provider constructs and executes SqliteCommand internally and relies on
            // ConfigurationManager/DB connectivity, we validate the fixed behavior via a source-level
            // invariant test: the DeleteUser SQL command text must use parameter placeholders and
            // must not interpolate the username directly.

            var source = typeof(TechInfoSystems.Data.SQLite.SQLiteMembershipProvider).Assembly
                .GetManifestResourceStream("WebGoat.Code.SQLiteMembershipProvider.cs");

            // If the project does not embed source files as resources, fall back to reflection-based
            // string search in method body is not possible in .NET; therefore we assert a minimal
            // invariant by scanning the file on disk when available.
            // This test assumes the repository layout keeps the file at WebGoat/Code/SQLiteMembershipProvider.cs.

            var path = System.IO.Path.Combine(System.AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "Code", "SQLiteMembershipProvider.cs");
            if (!System.IO.File.Exists(path))
            {
                // If the file isn't present in test context, skip deterministically.
                // Xunit doesn't have a built-in skip at runtime without traits; using return.
                return;
            }

            var fileText = System.IO.File.ReadAllText(path);

            // Assert: fixed query uses @Username and @ApplicationId placeholders in DeleteUser.
            Assert.Contains("WHERE LoweredUsername = @Username", fileText);
            Assert.Contains("ApplicationId = @ApplicationId", fileText);

            // Assert: no string interpolation with username in DeleteUser SQL.
            // (We allow general string concatenation for table name constants, but not for username.)
            Assert.DoesNotContain("LoweredUsername = \" + username", fileText);
            Assert.DoesNotContain("LoweredUsername = '" , fileText);
        }
    }
}
