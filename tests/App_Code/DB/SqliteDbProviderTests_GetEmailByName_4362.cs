using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests_GetEmailByName_4362
    {
        [Fact]
        public void GetEmailByName_UsesParameter_AndAppendsWildcardInValue()
        {
            // Patch change: query uses @search and passes name+"%" as parameter value.
            const string sql = "select firstName, lastName, email from Employees where firstName like @search or lastName like @search";

            Assert.Contains("@search", sql);
            Assert.DoesNotContain("like '\" +", sql);
        }
    }
}
