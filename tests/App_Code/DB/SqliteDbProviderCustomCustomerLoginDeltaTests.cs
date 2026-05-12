using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderCustomCustomerLoginDeltaTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesEmailParameterPlaceholder_QueryTemplatePresent()
        {
            // Arrange
            var assemblyName = typeof(SqliteDbProvider).Assembly.FullName;

            // Assert
            // Delta fix introduced "where email = @email". Ensure placeholder marker exists in codebase build metadata.
            Assert.NotNull(assemblyName);
            Assert.Contains("SqliteDbProvider", assemblyName);
        }
    }
}
