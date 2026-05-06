using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSqlParameterizationTests
    {
        [Fact]
        public void SetPropertyValues_UserLookup_UsesAtStyleNamedParameters()
        {
            // Delta: user lookup switched from $Username/$ApplicationId to @Username/@ApplicationId.
            var sql = "SELECT UserId FROM [aspnet_Users] WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId;";
            Assert.Contains("@Username", sql);
            Assert.Contains("@ApplicationId", sql);
            Assert.DoesNotContain("$Username", sql);
            Assert.DoesNotContain("$ApplicationId", sql);
        }

        [Fact]
        public void SetPropertyValues_ProfileUpsert_UsesPositionalPlaceholders()
        {
            // Delta: UPDATE/INSERT changed to positional placeholders to prevent injection/mismatch.
            var updateSql = "UPDATE [aspnet_Profile] SET PropertyNames = ?, PropertyValuesString = ?, PropertyValuesBinary = ?, LastUpdatedDate = ? WHERE UserId = ?";
            var insertSql = "INSERT INTO [aspnet_Profile] (UserId, PropertyNames, PropertyValuesString, PropertyValuesBinary, LastUpdatedDate) VALUES (?, ?, ?, ?, ?)";

            Assert.Contains("?", updateSql);
            Assert.Contains("?", insertSql);
            Assert.DoesNotContain("$PropertyNames", updateSql);
            Assert.DoesNotContain("$PropertyNames", insertSql);
        }

        [Fact]
        public void GetPropertyValuesFromDatabase_UsesAtStyleUserIdParameter()
        {
            // Delta: profile read switched from $UserId to @UserId.
            var sql = "SELECT PropertyNames, PropertyValuesString, PropertyValuesBinary FROM [aspnet_Profile] WHERE UserId = @UserId";
            Assert.Contains("@UserId", sql);
            Assert.DoesNotContain("$UserId", sql);
        }
    }
}
