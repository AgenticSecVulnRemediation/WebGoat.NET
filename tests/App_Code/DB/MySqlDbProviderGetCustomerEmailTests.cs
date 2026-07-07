using Xunit;

// Assumption: production code namespace as per file.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_UsesCustomerNumberParameter()
        {
            // Arrange
            // Delta test for PR #4020: customerNumber concatenation replaced with @customerNumber parameter.
            var asm = typeof(MySqlDbProvider).Assembly;
            var bytes = System.IO.File.ReadAllBytes(asm.Location);
            var text = System.Text.Encoding.UTF8.GetString(bytes);

            // Act / Assert
            Assert.Contains("where customerNumber = @customerNumber", text);
            Assert.Contains("AddWithValue(\"@customerNumber\"", text);
            Assert.DoesNotContain("where customerNumber = \" + customerNumber", text);
        }
    }
}
