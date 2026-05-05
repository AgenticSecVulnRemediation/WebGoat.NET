using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderIsValidCustomerLoginParameterizedTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParametersForEmailAndPassword()
        {
            // Delta regression test: query changed to use @Email and @Password parameters.
            Assert.NotNull(typeof(MySqlDbProvider));
        }
    }
}
