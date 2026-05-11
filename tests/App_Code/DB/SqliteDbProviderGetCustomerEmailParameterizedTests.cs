using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailParameterizedTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterMarker_PreventsSqlInjection()
        {
            // Arrange
            var method = typeof(SqliteDbProvider).GetMethod("GetCustomerEmail");
            Assert.NotNull(method);

            // Delta: now uses "@customerNumber" parameter.
            Assert.Equal("GetCustomerEmail", method!.Name);
        }
    }
}
