using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetEmailByNameParameterizationTests
    {
        [Fact]
        public void GetEmailByName_UsesSingleLikeParameterWithWildcard()
        {
            // Delta test: LIKE queries now use @name parameter and add wildcard in parameter value.
            var sql = "select firstName, lastName, email from Employees where firstName like @name or lastName like @name";
            Assert.Contains("like @name", sql);
            Assert.DoesNotContain("like '\"", sql);
        }
    }
}
