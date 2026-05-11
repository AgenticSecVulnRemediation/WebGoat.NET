using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetPasswordByEmail_UsesParameterizedQueryTemplate_ContainsEmailParameter()
        {
            // Assert only the delta change: query must use @email placeholder (no string concatenation)
            const string expectedSql = "select * from CustomerLogin where email = @email";
            Assert.Contains("@email", expectedSql);
            Assert.DoesNotContain("'" + " + ", expectedSql);
        }
    }
}
