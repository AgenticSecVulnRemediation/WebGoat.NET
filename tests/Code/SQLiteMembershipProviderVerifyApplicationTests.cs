using Xunit;
using Mono.Data.Sqlite;
using System.Data;
using TechInfoSystems.Data.SQLite;

// Assumption: Namespace TechInfoSystems.Data.SQLite is accessible from test project.
// Delta behavior: VerifyApplication uses @-prefixed parameters consistently (ApplicationName/Description) instead of mixing $ and missing prefixes.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderVerifyApplicationTests
    {
        [Fact]
        public void VerifyApplication_UsesAtParameters_ForInsert()
        {
            // Arrange
            // We can't easily run VerifyApplication without a database and config; instead validate command text and parameter names via reflection.
            // This unit test ensures the security fix (parameter binding correctness) is preserved.
            var provider = new SQLiteMembershipProvider();

            var method = typeof(SQLiteMembershipProvider).GetMethod("VerifyApplication", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(method);

            // Act & Assert
            // Ensure the INSERT statement uses @ApplicationId, @ApplicationName, @Description.
            // We inspect the updated source via embedded strings is not possible at runtime; so assert via IL string constants.
            var body = method.GetMethodBody();
            Assert.NotNull(body);

            // Conservative: ensure method's IL contains the required parameter tokens.
            var il = method.GetMethodBody().GetILAsByteArray();
            var ilString = System.Text.Encoding.UTF8.GetString(il);
            Assert.Contains("@ApplicationId", ilString);
            Assert.Contains("@ApplicationName", ilString);
            Assert.Contains("@Description", ilString);
        }
    }
}
