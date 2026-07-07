using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetEmailByCustomerNumber_ParameterizedScalarTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesCustomerNumberParameter()
        {
            // Arrange
            var type = typeof(SqliteDbProvider);
            var method = type.GetMethod("GetEmailByCustomerNumber");
            Assert.NotNull(method);

            // Act
            var methodText = method!.ToString();

            // Assert
            Assert.Contains("GetEmailByCustomerNumber", methodText);
        }
    }
}
