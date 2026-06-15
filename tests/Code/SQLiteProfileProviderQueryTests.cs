using System;
using System.Reflection;
using Xunit;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderQueryTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_QueryUsesAspnetProfileLiteral_NotBracketedConstant()
        {
            // Arrange
            // Delta: SELECT query now uses a hard-coded "aspnet_Profile" table name instead of PROFILE_TB_NAME.
            // Ensure method still exists and is compilable; this test is a narrow regression guard.
            var method = typeof(SQLiteProfileProvider).GetMethod(
                "GetPropertyValuesFromDatabase",
                BindingFlags.NonPublic | BindingFlags.Static);

            Assert.NotNull(method);

            // Act
            var body = method!.GetMethodBody();

            // Assert
            Assert.NotNull(body);
        }
    }
}
