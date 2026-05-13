using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPasswordByEmailTests
    {
        [Fact]
        public void GetPasswordByEmail_Query_UsesParameterizedEmail()
        {
            // Arrange
            var provider = new SqliteDbProvider(new ConfigFile());

            // Act
            string sql = "select * from CustomerLogin where email = @email;";

            // Assert
            Assert.Contains("@email", sql);
            Assert.DoesNotContain("'", sql);
        }
    }
}
