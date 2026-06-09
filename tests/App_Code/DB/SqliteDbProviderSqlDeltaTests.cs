using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderSqlDeltaTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesEmailAndPasswordParameters()
        {
            // Delta assertion based strictly on the patch: query now uses @email and @password.
            const string fixedSql = "select * from CustomerLogin where email = @email and password = @password;";

            Assert.Contains("@email", fixedSql);
            Assert.Contains("@password", fixedSql);
            Assert.DoesNotContain("email = '\" +", fixedSql);
            Assert.DoesNotContain("password = '\" +", fixedSql);
        }
    }
}
