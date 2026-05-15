using Xunit;
using System.IO;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPaymentsTests
    {
        [Fact]
        public void GetPayments_UsesParameterizedQuery_ForCustomerNumber()
        {
            // Arrange
            var source = File.ReadAllText("WebGoat/App_Code/DB/MySqlDbProvider.cs");

            // Act & Assert
            Assert.Contains("select * from Payments where customerNumber = @customerNumber", source);
            Assert.Contains("cmd.Parameters.AddWithValue(\"@customerNumber\"", source);
            Assert.DoesNotContain("customerNumber = \" + customerNumber", source);
        }
    }
}
