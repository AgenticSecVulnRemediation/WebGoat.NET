using Xunit;

// Assumption: Production class is OWASP.WebGoat.NET.App_Code.DB.SqliteDbProvider.
// Delta test: GetCustomerEmail now uses a parameter for customerNumber.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetCustomerEmail_Tests
    {
        [Fact]
        public void GetCustomerEmail_MethodExists_AfterParameterizationFix()
        {
            var method = typeof(OWASP.WebGoat.NET.App_Code.DB.SqliteDbProvider)
                .GetMethod("GetCustomerEmail");

            Assert.NotNull(method);
            var parameters = method!.GetParameters();
            Assert.Single(parameters);
            Assert.Equal(typeof(string), parameters[0].ParameterType);
        }
    }
}
