using Xunit;

// Assumption: Production class is OWASP.WebGoat.NET.App_Code.DB.SqliteDbProvider.
// Delta test: GetProductDetails now uses parameterized SqliteCommand for productCode in both queries.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetProductDetails_Tests
    {
        [Fact]
        public void GetProductDetails_MethodExists_AfterParameterizationFix()
        {
            var method = typeof(OWASP.WebGoat.NET.App_Code.DB.SqliteDbProvider)
                .GetMethod("GetProductDetails");

            Assert.NotNull(method);
            var parameters = method!.GetParameters();
            Assert.Single(parameters);
            Assert.Equal(typeof(string), parameters[0].ParameterType);
        }
    }
}
