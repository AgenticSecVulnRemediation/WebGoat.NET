using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPasswordByEmailTests
    {
        [Fact]
        public void GetPasswordByEmail_SqlString_UsesEmailParameter()
        {
            // Arrange/Act
            string source = typeof(MySqlDbProvider).ToString();

            // Assert (delta)
            Assert.Contains("where email = @email", source);
        }
    }
}
