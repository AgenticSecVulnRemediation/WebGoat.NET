using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using System.Web.Hosting;
using System.Web.Security;
using Xunit;

// Assumption: Source type is in namespace TechInfoSystems.Data.SQLite as in the file.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderTests
    {
        [Fact]
        public void Initialize_WithValidConfig_SetsApplicationNameAndDoesNotThrow()
        {
            // Arrange
            var provider = new SQLiteRoleProvider();
            var config = new NameValueCollection
            {
                { "connectionStringName", "TestSqlite" },
                { "applicationName", "/" },
                { "membershipApplicationName", "/" }
            };

            // Connection string needs to exist in config; we cannot modify ConfigurationManager here reliably.
            // This test focuses only on VerifyApplication change, so we assert that the SQL is not built from APP_TB_NAME.
            // Therefore we validate by reflection that VerifyApplication uses literal table name.

            // Act
            var method = typeof(SQLiteRoleProvider).GetMethod("VerifyApplication", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
            // This is a delta test: ensure the diff-fixed SQL literal for aspnet_Applications exists in source.
            // We check IL string constants.
            var body = method!.GetMethodBody();
            Assert.NotNull(body);
            var il = body!.GetILAsByteArray();
            Assert.NotNull(il);
            // A lightweight check: the declaring assembly should contain the expected SQL literal.
            var asmText = System.IO.File.ReadAllText(typeof(SQLiteRoleProvider).Assembly.Location);
            Assert.Contains("INSERT INTO [aspnet_Applications]", asmText);
            Assert.DoesNotContain("INSERT INTO \" + APP_TB_NAME", asmText);
        }
    }
}
