using Xunit;

// Assumption: Production class is OWASP.WebGoat.NET.App_Code.DB.MySqlDbProvider.
// Delta test: GetOrders now uses a parameterized query for customerNumber.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_GetOrders_Tests
    {
        [Fact]
        public void GetOrders_MethodSignature_RemainsStable_AfterParameterizationFix()
        {
            // Arrange
            var method = typeof(OWASP.WebGoat.NET.App_Code.DB.MySqlDbProvider)
                .GetMethod("GetOrders");

            // Assert
            Assert.NotNull(method);
            var parameters = method!.GetParameters();
            Assert.Single(parameters);
            Assert.Equal(typeof(int), parameters[0].ParameterType);
        }
    }
}
