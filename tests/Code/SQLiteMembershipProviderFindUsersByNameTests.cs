using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderFindUsersByNameTests
    {
        [Fact]
        public void FindUsersByName_WithCommaInUsernameToMatch_ThrowsArgumentException_BeforeDbAccess()
        {
            // Arrange
            // Provide an in-memory SQLite connection string in config to avoid provider init failures.
            // This keeps the test deterministic and ensures we reach the input validation logic.
            var config = new NameValueCollection
            {
                { "connectionStringName", "MembershipDb" },
                { "applicationName", "TestApp" },
                { "enablePasswordReset", "false" },
                { "enablePasswordRetrieval", "false" },
                { "requiresQuestionAndAnswer", "false" },
                { "requiresUniqueEmail", "false" },
                { "passwordFormat", "Hashed" }
            };

            // Ensure connection string exists at runtime.
            // If the test host doesn't allow injecting ConfigurationManager.ConnectionStrings,
            // we fall back to setting private field via reflection.
            var provider = new SQLiteMembershipProvider();

            TrySetProviderConnectionString(provider, "Data Source=:memory:;Version=3;New=True;");

            // Provider Initialize reads ConnectionStrings; it may still throw if not present. In that case,
            // the test is inconclusive for the validation path and should fail loudly.
            try
            {
                provider.Initialize("SQLiteMembershipProvider", config);
            }
            catch (Exception ex)
            {
                throw new Xunit.Sdk.XunitException(
                    "Provider initialization failed; test requires ability to use an in-memory connection string. " +
                    "Init exception: " + ex);
            }

            // Act + Assert
            // The security-relevant behavior: reject commas in usernameToMatch.
            Assert.Throws<ArgumentException>(() => provider.FindUsersByName("a,b", 0, 10, out _));
        }

        private static void TrySetProviderConnectionString(SQLiteMembershipProvider provider, string connectionString)
        {
            // Best-effort to avoid dependency on machine/web.config.
            var field = typeof(SQLiteMembershipProvider).GetField("_connectionString", BindingFlags.NonPublic | BindingFlags.Static);
            if (field != null)
            {
                field.SetValue(null, connectionString);
            }
        }
    }
}
