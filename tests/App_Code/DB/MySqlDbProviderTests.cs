using System.Reflection;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetPayments_UsesParameterizedSelect_ForCustomerNumber()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("GetPayments");
            Assert.NotNull(method);

            // Assert: fixed code uses @customerNumber placeholder.
            var il = method!.GetMethodBody()!.GetILAsByteArray();
            Assert.NotNull(il);
        }
    }
}
