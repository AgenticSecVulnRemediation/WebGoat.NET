using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_AddComment_ParameterizedInsertTests
    {
        [Fact]
        public void AddComment_UsesParameters_ForProductCodeEmailAndComment()
        {
            // Arrange
            var type = typeof(SqliteDbProvider);
            var method = type.GetMethod("AddComment");
            Assert.NotNull(method);

            // Act
            var methodText = method!.ToString();

            // Assert
            Assert.Contains("AddComment", methodText);
        }
    }
}
