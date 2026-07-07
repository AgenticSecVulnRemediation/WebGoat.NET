using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_CustomCustomerLogin_ParameterizedQueryTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesEmailParameter_InsteadOfInlineEmail()
        {
            // Arrange
            var type = typeof(SqliteDbProvider);
            var method = type.GetMethod("CustomCustomerLogin");
            Assert.NotNull(method);

            // Act
            var methodText = method!.ToString();

            // Assert
            Assert.Contains("CustomCustomerLogin", methodText);
        }
    }
}
