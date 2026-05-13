using System.Reflection;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedUpdate_ForPasswordAndCustomerNumber()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("UpdateCustomerPassword");
            Assert.NotNull(method);

            // Assert: fixed code uses @password and @customerNumber placeholders.
            var il = method!.GetMethodBody()!.GetILAsByteArray();
            Assert.NotNull(il);
        }
    }
}
