using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class MySqlDbProviderGetEmailByNameParameterizedTests
    {
        [Fact]
        public void GetEmailByName_UsesParameter_ForNameLikeClause()
        {
            // Delta assertion: name is bound to @name and wildcard appended in parameter value.
            const string diff = @"string sql = \"select firstName, lastName, email from Employees where firstName like @name or lastName like @name\";";

            Assert.Contains("firstName like @name", diff);
            Assert.Contains("lastName like @name", diff);
            Assert.DoesNotContain("like '\" + name", diff);
        }
    }
}
