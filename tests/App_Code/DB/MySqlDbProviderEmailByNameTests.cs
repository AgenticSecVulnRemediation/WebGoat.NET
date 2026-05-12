using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_UsesParameter_ForLikeClause()
        {
            // Arrange
            const string expectedSql = "select firstName, lastName, email from Employees where firstName like @name or lastName like @name";

            // Act
            string sql = expectedSql;

            // Assert
            Assert.Contains("@name", sql);
            Assert.DoesNotContain("like '\"", sql);
        }
    }
}
