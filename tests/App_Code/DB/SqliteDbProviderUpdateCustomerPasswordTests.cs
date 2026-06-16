using Xunit;

// Assumption: Production class is OWASP.WebGoat.NET.App_Code.DB.SqliteDbProvider.
// Delta test: UpdateCustomerPassword now uses parameters.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_UpdateCustomerPassword_Tests
    {
        [Fact]
        public void UpdateCustomerPassword_MethodSignature_RemainsStable_AfterParameterizationFix()
        {
            var method = typeof(OWASP.WebGoat.NET.App_Code.DB.SqliteDbProvider)
                .GetMethod("UpdateCustomerPassword");

            Assert.NotNull(method);
            var parameters = method!.GetParameters();
            Assert.Equal(2, parameters.Length);
            Assert.Equal(typeof(int), parameters[0].ParameterType);
            Assert.Equal(typeof(string), parameters[1].ParameterType);
        }
    }
}
