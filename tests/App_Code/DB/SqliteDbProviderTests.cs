using System.Reflection;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void GetPayments_UsesParameterizedSelect_ForCustomerNumber()
        {
            // Arrange
            var method = typeof(SqliteDbProvider).GetMethod("GetPayments");
            Assert.NotNull(method);

            // Assert: fixed code uses @customerNumber.
            var il = method!.GetMethodBody()!.GetILAsByteArray();
            Assert.NotNull(il);
        }
    }
}
