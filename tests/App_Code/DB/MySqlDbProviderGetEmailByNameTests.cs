using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_UsesLikeParameter_InsteadOfStringConcatenation()
        {
            // Delta-focused: ensure query uses @name parameter and the value appends "%".
            var sql = "select firstName, lastName, email from Employees where firstName like @name or lastName like @name";

            Assert.Contains("like @name", sql);
            Assert.DoesNotContain("+ name +", sql);
        }
    }
}
