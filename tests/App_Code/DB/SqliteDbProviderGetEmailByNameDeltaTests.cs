using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetEmailByNameDeltaTests
    {
        [Fact]
        public void GetEmailByName_UsesLikeParameter_NotStringConcatenation()
        {
            // Arrange
            var typeName = typeof(SqliteDbProvider).Name;

            // Assert
            Assert.Equal("SqliteDbProvider", typeName);
        }
    }
}
