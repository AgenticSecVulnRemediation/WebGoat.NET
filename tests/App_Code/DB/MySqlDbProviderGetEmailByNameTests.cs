using Xunit;

namespace OWASP.WebGoat.NET.Tests.App_Code.DB
{
    public class MySqlDbProviderGetEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_UsesParameterizedLike()
        {
            // Arrange
            const string sql = "select firstName, lastName, email from Employees where firstName like @name or lastName like @name";

            // Assert
            Assert.Contains("like @name", sql);
            Assert.DoesNotContain("like '\" + name", sql);
        }
    }
}
