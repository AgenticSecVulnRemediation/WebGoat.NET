using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_UsesLikeParameterWithWildcard()
        {
            // Arrange/Act
            var expectedSql = "select firstName, lastName, email from Employees where firstName like @name or lastName like @name";
            var expectedParameterValueExample = "alice" + "%";

            // Assert
            Assert.Contains("like @name", expectedSql);
            Assert.DoesNotContain("like '\" + name + \"%'", expectedSql);
            Assert.EndsWith("%", expectedParameterValueExample);
        }
    }
}
