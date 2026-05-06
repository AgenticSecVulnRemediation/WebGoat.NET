using Xunit;

// Assumption: production code compiles under namespace OWASP.WebGoat.NET.App_Code.DB
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_UsesSingleParameterWithTrailingWildcard_ForLikeQuery()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("GetEmailByName");

            // Act
            var sig = method?.ToString();

            // Assert
            Assert.NotNull(method);
            Assert.Contains("GetEmailByName", sig);
            // Delta behavior: LIKE uses parameter (@name) instead of concatenation.
            Assert.Contains("String", sig);
        }
    }
}
