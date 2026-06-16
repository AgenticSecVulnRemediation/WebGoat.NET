using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderCustomerNumberQueriesParameterizationTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterForCustomerNumber()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("GetCustomerEmail");
            Assert.NotNull(method);

            // Assert
            Assert.Contains("where customerNumber = @customerNumber",
                System.IO.File.ReadAllText(method!.Module.FullyQualifiedName));
        }

        [Fact]
        public void GetEmailByCustomerNumber_UsesMySqlParameter()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("GetEmailByCustomerNumber");
            Assert.NotNull(method);

            // Assert
            Assert.Contains("new MySqlParameter(\"@num\"",
                System.IO.File.ReadAllText(method!.Module.FullyQualifiedName));
        }
    }
}
