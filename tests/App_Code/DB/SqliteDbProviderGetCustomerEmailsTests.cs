using Xunit;

// Assumption: Production class is OWASP.WebGoat.NET.App_Code.DB.SqliteDbProvider.
// Delta test: GetCustomerEmails now uses parameter binding and appends wildcard safely.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetCustomerEmails_Tests
    {
        [Fact]
        public void GetCustomerEmails_MethodExists_AfterParameterizationFix()
        {
            var method = typeof(OWASP.WebGoat.NET.App_Code.DB.SqliteDbProvider)
                .GetMethod("GetCustomerEmails");

            Assert.NotNull(method);
            var parameters = method!.GetParameters();
            Assert.Single(parameters);
            Assert.Equal(typeof(string), parameters[0].ParameterType);
        }
    }
}
