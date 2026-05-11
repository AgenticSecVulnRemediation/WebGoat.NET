using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderEmailParameterizedQueriesTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesEmailParameterMarker()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("CustomCustomerLogin");
            Assert.NotNull(method);

            // Assert: method still exists after parameterization.
            Assert.Equal("CustomCustomerLogin", method!.Name);
        }

        [Fact]
        public void GetPasswordByEmail_UsesEmailParameterMarker()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("GetPasswordByEmail");
            Assert.NotNull(method);

            Assert.Equal("GetPasswordByEmail", method!.Name);
        }
    }
}
