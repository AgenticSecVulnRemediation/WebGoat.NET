using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderCustomCustomerLoginParameterizationTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterizedEmailQuery()
        {
            // Arrange
            var method = typeof(SqliteDbProvider).GetMethod("CustomCustomerLogin");
            Assert.NotNull(method);

            // Assert
            var text = System.IO.File.ReadAllText(method!.Module.FullyQualifiedName);
            Assert.Contains("where email = @email", text);
            Assert.Contains("Parameters.AddWithValue(\"@email\"", text);
        }
    }
}
