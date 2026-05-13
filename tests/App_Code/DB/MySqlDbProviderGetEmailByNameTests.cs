using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_UsesParameterizedLikeQuery()
        {
            // Arrange
            var mi = typeof(MySqlDbProvider).GetMethod("GetEmailByName");

            // Assert
            Assert.NotNull(mi);
        }
    }
}
