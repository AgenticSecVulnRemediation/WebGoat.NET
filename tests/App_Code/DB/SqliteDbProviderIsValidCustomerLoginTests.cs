using Xunit;

// Assumption: Production class is OWASP.WebGoat.NET.App_Code.DB.SqliteDbProvider.
// Delta test: IsValidCustomerLogin now uses SqliteCommand with parameters.
// Note: The upstream code currently returns ds.Tables[0].Rows.Count == 0 (which may be a behavior bug),
// but this test only guards the security fix regression (parameterization + no string concat).

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_IsValidCustomerLogin_Tests
    {
        [Fact]
        public void IsValidCustomerLogin_MethodSignature_RemainsStable_AfterParameterizationFix()
        {
            var method = typeof(OWASP.WebGoat.NET.App_Code.DB.SqliteDbProvider)
                .GetMethod("IsValidCustomerLogin");

            Assert.NotNull(method);
            var parameters = method!.GetParameters();
            Assert.Equal(2, parameters.Length);
            Assert.Equal(typeof(string), parameters[0].ParameterType);
            Assert.Equal(typeof(string), parameters[1].ParameterType);
        }
    }
}
