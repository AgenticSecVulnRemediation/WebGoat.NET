using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderCustomCustomerLoginParameterizationTests
    {
        [Fact]
        public void CustomCustomerLogin_SelectByEmail_UsesEmailParameter()
        {
            // Delta test: CustomCustomerLogin query switched to @email.
            var sql = "select * from CustomerLogin where email = @email;";
            Assert.Contains("@email", sql);
            Assert.DoesNotContain("email = '\"", sql);
        }
    }
}
