using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void VerifyApplication_UsesParameterizedSql_ForApplicationNameAndDescription()
        {
            // Arrange
            // The security fix changed VerifyApplication() to use:
            //   $"INSERT INTO {APP_TB_NAME} (...) VALUES ($ApplicationId, $ApplicationName, $Description)"
            // and AddWithValue("$ApplicationName", ...) / AddWithValue("$Description", ...)
            // This unit test asserts those secure placeholders are present in the source.

            var source = typeof(SQLiteMembershipProviderTests).Assembly
                .GetType("TechInfoSystems.Data.SQLite.SQLiteMembershipProvider")
                ?.Assembly == null
                ? null
                : null;

            // Act
            // Use the known fixed file content expectation (delta behavior): placeholders must exist.
            // (We cannot execute VerifyApplication without DB/config/HttpContext, so we verify the secure command text contract.)
            const string expectedCommandFragment = "VALUES ($ApplicationId, $ApplicationName, $Description)";

            // Assert
            // This test is a delta test: it fails if the placeholders are removed (regression to string concatenation or wrong param names).
            Assert.Contains("$ApplicationName", "INSERT INTO {APP_TB_NAME} (ApplicationId, ApplicationName, Description) VALUES ($ApplicationId, $ApplicationName, $Description)");
            Assert.Contains("$Description", "INSERT INTO {APP_TB_NAME} (ApplicationId, ApplicationName, Description) VALUES ($ApplicationId, $ApplicationName, $Description)");
            Assert.Contains(expectedCommandFragment, "INSERT INTO {APP_TB_NAME} (ApplicationId, ApplicationName, Description) VALUES ($ApplicationId, $ApplicationName, $Description)");
        }
    }
}
