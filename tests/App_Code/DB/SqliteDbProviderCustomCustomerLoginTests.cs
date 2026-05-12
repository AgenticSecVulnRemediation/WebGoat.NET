using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderCustomCustomerLoginTests
    {
        [Fact]
        public void CustomCustomerLogin_DoesNotIntroduceUndefinedVariables()
        {
            // Delta test: the change attempted to add a parameter using an undefined variable ("name").
            // This test asserts the code compiles by referencing the provider type.
            // If the source contains undefined identifiers, the build will fail and this test file will never compile.
            Assert.True(typeof(OWASP.WebGoat.NET.App_Code.DB.SqliteDbProvider) != null);
        }
    }
}
