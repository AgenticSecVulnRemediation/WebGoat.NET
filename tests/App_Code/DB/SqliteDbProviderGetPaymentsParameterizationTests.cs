using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPaymentsParameterizationTests
    {
        [Fact]
        public void GetPayments_UsesParameterForCustomerNumber()
        {
            // Arrange
            var method = typeof(SqliteDbProvider).GetMethod("GetPayments");
            Assert.NotNull(method);

            // Assert
            Assert.Contains("customerNumber = @customerNumber", System.IO.File.ReadAllText(method!.Module.FullyQualifiedName));
        }
    }
}
