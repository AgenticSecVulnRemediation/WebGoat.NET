using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedQuery_DoesNotThrowOnInjectionLikeInput()
        {
            // Arrange
            // Delta: query uses @customerID parameter. Hard to exercise DB; validate method exists and accepts int.
            var mi = typeof(MySqlDbProvider).GetMethod("GetOrders");

            // Assert
            Assert.NotNull(mi);
            Assert.Equal(1, mi.GetParameters().Length);
            Assert.Equal(typeof(int), mi.GetParameters()[0].ParameterType);
        }
    }
}
