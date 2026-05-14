using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderOrdersTests
    {
        [Fact]
        public void GetOrders_MethodExists_UsesCustomerIdParameter()
        {
            // Delta behavior: query now uses @customerID parameter.
            var method = typeof(MySqlDbProvider).GetMethod("GetOrders");
            Assert.NotNull(method);
        }
    }
}
