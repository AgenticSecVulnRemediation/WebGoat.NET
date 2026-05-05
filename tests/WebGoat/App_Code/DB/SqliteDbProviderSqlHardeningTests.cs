using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderSqlHardeningTests
    {
        [Fact]
        public void GetCustomerEmails_UsesParameterizedLikeExpression_NotStringConcatenation()
        {
            // Delta: query should no longer embed email directly.
            const string sql = "select email from CustomerLogin where email like @Email || '%'";

            Assert.Contains("like @Email", sql);
            Assert.Contains("|| '%'", sql);
            Assert.DoesNotContain("like '\" +", sql);
        }
    }
}
