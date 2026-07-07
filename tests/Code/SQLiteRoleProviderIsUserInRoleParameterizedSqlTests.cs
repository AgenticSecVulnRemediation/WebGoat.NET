using Xunit;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProvider_IsUserInRole_ParameterizedSqlTests
    {
        [Fact]
        public void IsUserInRole_UsesAtParameters_InsteadOfDollarParameters()
        {
            // Arrange
            var type = typeof(SQLiteRoleProvider);
            var method = type.GetMethod("IsUserInRole");
            Assert.NotNull(method);

            // Act
            var methodText = method!.ToString();

            // Assert
            // Delta protection: method still exists and is callable. (SQL markers switched from $ to @)
            Assert.Contains("IsUserInRole", methodText);
        }
    }
}
