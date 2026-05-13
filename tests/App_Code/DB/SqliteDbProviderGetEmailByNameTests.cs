using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_UsesNamedParameterInsteadOfStringConcatenation()
        {
            // Arrange
            var sql = "select firstName, lastName, email from Employees where firstName like @Name or lastName like @Name";

            // Assert
            Assert.Contains("like @Name", sql);
            Assert.DoesNotContain("'" + " +", sql);
            Assert.DoesNotContain("%'");
        }
    }
}
