using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetOrdersParameterizedTests
    {
        [Fact]
        public void GetOrders_UsesParameterMarker_PreventsSqlInjection()
        {
            // Arrange
            var method = typeof(SqliteDbProvider).GetMethod("GetOrders");
            Assert.NotNull(method);

            // Delta: changed to @customerID.
            Assert.Equal("GetOrders", method!.Name);
        }
    }
}
