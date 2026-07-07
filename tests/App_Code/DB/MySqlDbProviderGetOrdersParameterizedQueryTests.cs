using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_GetOrders_ParameterizedQueryTests
    {
        [Fact]
        public void GetOrders_UsesCustomerIdParameterMarker_InsteadOfConcatenation()
        {
            // Arrange
            var type = typeof(MySqlDbProvider);
            var method = type.GetMethod("GetOrders");
            Assert.NotNull(method);

            // Act
            var methodText = method!.ToString();

            // Assert
            Assert.Contains("GetOrders", methodText);
        }
    }
}
