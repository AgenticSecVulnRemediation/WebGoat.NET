using System;
using System.Text;
using Xunit;

// Assumption: Source is in OWASP.WebGoat.NET.App_Code.DB namespace.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailParameterizedTests
    {
        // Delta test: GetCustomerEmail now uses @customerNumber parameter.
        [Fact]
        public void GetCustomerEmail_SourceContainsCustomerNumberParameter()
        {
            var asm = typeof(MySqlDbProvider).Assembly;
            var bytes = System.IO.File.ReadAllBytes(asm.Location);
            var text = Encoding.UTF8.GetString(bytes);

            Assert.Contains("select email from CustomerLogin where customerNumber = @customerNumber", text);
            Assert.Contains("command.Parameters.AddWithValue(\"@customerNumber\"", text);
            Assert.DoesNotContain("customerNumber = \" + customerNumber", text);
        }
    }
}
