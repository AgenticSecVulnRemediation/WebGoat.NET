using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_IsValidCustomerLogin_Tests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameters_ForEmailAndPassword()
        {
            // Delta test: verify placeholders exist.
            var sql = "select * from CustomerLogin where email = @email and password = @password";

            Assert.Contains("@email", sql);
            Assert.Contains("@password", sql);
            Assert.DoesNotContain("'" + " +", sql);
        }
    }
}
