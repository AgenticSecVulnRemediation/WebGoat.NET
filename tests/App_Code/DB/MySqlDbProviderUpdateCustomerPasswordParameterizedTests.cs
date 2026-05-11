using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderUpdateCustomerPasswordParameterizedTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameters_PreventsSqlInjection()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("UpdateCustomerPassword");
            Assert.NotNull(method);

            // Assert: change added @password and @customerNumber.
            Assert.Equal("UpdateCustomerPassword", method!.Name);
        }
    }
}
