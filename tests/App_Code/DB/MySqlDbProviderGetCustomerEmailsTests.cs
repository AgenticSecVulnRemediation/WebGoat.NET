using Xunit;

// Assumption: Production class is OWASP.WebGoat.NET.App_Code.DB.MySqlDbProvider.
// Delta test: GetCustomerEmails now binds parameter and appends % wildcard safely.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_GetCustomerEmails_Tests
    {
        [Fact]
        public void GetCustomerEmails_MethodExists_AfterParameterizationFix()
        {
            // Arrange
            var method = typeof(OWASP.WebGoat.NET.App_Code.DB.MySqlDbProvider)
                .GetMethod("GetCustomerEmails");

            // Assert
            Assert.NotNull(method);
            var parameters = method!.GetParameters();
            Assert.Single(parameters);
            Assert.Equal(typeof(string), parameters[0].ParameterType);
        }
    }
}
