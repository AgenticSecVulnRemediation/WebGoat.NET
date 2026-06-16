using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPaymentsParameterizationTests
    {
        [Fact]
        public void GetPayments_UsesParameterForCustomerNumber()
        {
            // Arrange
            // Avoid running a real DB call. Verify the SQL was updated to use @customerNumber.
            var method = typeof(MySqlDbProvider).GetMethod("GetPayments");
            Assert.NotNull(method);

            // Assert
            Assert.Contains("customerNumber = @customerNumber",
                System.IO.File.ReadAllText(method!.Module.FullyQualifiedName));
        }
    }
}
