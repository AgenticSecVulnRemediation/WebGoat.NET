using System;
using System.Reflection;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderVerifyApplicationParameterizationTests
    {
        [Fact]
        public void VerifyApplication_Insert_UsesAtParameters()
        {
            // Arrange/Act
            var sql = "INSERT INTO [aspnet_Applications] (ApplicationId, ApplicationName, Description) VALUES (@ApplicationId, @ApplicationName, @Description)";

            // Assert
            Assert.Contains("@ApplicationId", sql);
            Assert.Contains("@ApplicationName", sql);
            Assert.Contains("@Description", sql);
            Assert.DoesNotContain("$ApplicationId", sql);
        }
    }
}
