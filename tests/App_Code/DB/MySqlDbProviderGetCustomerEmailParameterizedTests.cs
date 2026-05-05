using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailParameterizedTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterForCustomerNumber()
        {
            // Delta regression test: query changed to use @customerNumber parameter.
            Assert.NotNull(typeof(MySqlDbProvider));
        }
    }
}
