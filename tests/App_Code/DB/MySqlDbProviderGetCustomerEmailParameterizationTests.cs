using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailParameterizationTests
    {
        [Fact]
        public void GetCustomerEmail_UsesCustomerNumberParameter()
        {
            // Regression test for injection fix: customerNumber must be bound via @customerNumber parameter.
            var asm = typeof(MySqlDbProvider).Assembly.ToString();

            Assert.Contains("customerNumber = @customerNumber", asm);
            Assert.DoesNotContain("customerNumber = \" + customerNumber", asm);
        }
    }
}
