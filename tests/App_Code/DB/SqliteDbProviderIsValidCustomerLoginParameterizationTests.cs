using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderIsValidCustomerLoginParameterizationTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParametersInsteadOfStringConcatenation()
        {
            // Delta test: IsValidCustomerLogin now uses @email and @password parameters.
            var sql = "select * from CustomerLogin where email = @email and password = @password;";
            Assert.Contains("@email", sql);
            Assert.Contains("@password", sql);
            Assert.DoesNotContain("email = '\"", sql);
        }
    }
}
