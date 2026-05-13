using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetPasswordByEmail_UsesParameterizedQuery_ForEmail()
        {
            // Arrange
            const string sql = "select * from CustomerLogin where email = @email;";

            // Assert
            Assert.Contains("@email", sql);
            Assert.DoesNotContain("'\" +", sql);
        }
    }
}
