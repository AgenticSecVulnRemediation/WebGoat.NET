using Xunit;

// Assumption: production code namespace as per file.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedCustomerNumber()
        {
            // Arrange
            // Delta test for PR #4026: Orders query now uses @customerNumber parameter.
            var asm = typeof(SqliteDbProvider).Assembly;
            var bytes = System.IO.File.ReadAllBytes(asm.Location);
            var text = System.Text.Encoding.UTF8.GetString(bytes);

            // Act / Assert
            Assert.Contains("where customerNumber = @customerNumber", text);
            Assert.Contains("AddWithValue(\"@customerNumber\"", text);
            Assert.DoesNotContain("where customerNumber = \" + customerID", text);
        }
    }
}
