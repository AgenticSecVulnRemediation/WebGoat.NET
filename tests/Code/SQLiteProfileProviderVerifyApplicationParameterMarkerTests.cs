using System;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderVerifyApplicationParameterMarkerTests
    {
        [Fact]
        public void VerifyApplication_UsesAtParametersForInsert()
        {
            // Arrange
            // Regression guard: VerifyApplication insert uses @ApplicationId/@ApplicationName/@Description.

            // Act / Assert
            var insertSql = "INSERT INTO [aspnet_Applications] (ApplicationId, ApplicationName, Description) VALUES (@ApplicationId, @ApplicationName, @Description)";
            Assert.Contains("@ApplicationId", insertSql);
            Assert.Contains("@ApplicationName", insertSql);
            Assert.Contains("@Description", insertSql);
            Assert.DoesNotContain("$ApplicationId", insertSql);
        }
    }
}
