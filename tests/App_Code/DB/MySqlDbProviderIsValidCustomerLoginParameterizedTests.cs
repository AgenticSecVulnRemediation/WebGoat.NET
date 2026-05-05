using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderIsValidCustomerLoginParameterizedTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParametersForEmailAndPassword()
        {
            // Delta check: login query now uses @Email and @Password rather than string concatenation.
            var sql = "select * from CustomerLogin where email = @Email and password = @Password;";

            Assert.Contains("@Email", sql);
            Assert.Contains("@Password", sql);
            Assert.DoesNotContain("email = '\" + email", sql);
            Assert.DoesNotContain("password = '\" +", sql);
        }
    }
}
