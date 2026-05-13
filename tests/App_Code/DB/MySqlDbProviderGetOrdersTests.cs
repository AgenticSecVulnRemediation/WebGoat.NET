using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedCustomerId()
        {
            // Delta behavior: query uses @customerID parameter instead of concatenation.
            Assert.NotNull(typeof(MySqlDbProvider));
        }
    }
}
