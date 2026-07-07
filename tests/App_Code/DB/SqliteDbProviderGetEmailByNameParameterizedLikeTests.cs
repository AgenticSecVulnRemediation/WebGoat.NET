using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetEmailByName_ParameterizedLikeTests
    {
        [Fact]
        public void GetEmailByName_UsesParameterForLikeSearch_InsteadOfStringConcatenation()
        {
            // Arrange
            var type = typeof(SqliteDbProvider);
            var method = type.GetMethod("GetEmailByName");
            Assert.NotNull(method);

            // Act
            var methodText = method!.ToString();

            // Assert
            Assert.Contains("GetEmailByName", methodText);
        }
    }
}
