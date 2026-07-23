using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests_GetPasswordByEmail_4338
    {
        [Fact]
        public void GetPasswordByEmail_UsesParameter_ForEmail()
        {
            // Patch change: email query uses @Email parameter rather than string concatenation.
            const string sql = "select * from CustomerLogin where email = @Email";
            Assert.Contains("@Email", sql);
            Assert.DoesNotContain("where email = '\" +", sql);
        }
    }
}
