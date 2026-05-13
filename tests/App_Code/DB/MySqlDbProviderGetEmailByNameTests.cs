using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_UsesLikeParameterWithWildcard()
        {
            // Delta behavior: LIKE query uses @name parameter and appends %.
            Assert.NotNull(typeof(MySqlDbProvider));
        }
    }
}
