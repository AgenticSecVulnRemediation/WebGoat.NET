using System;
using System.Text;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPaymentsParameterizationTests
    {
        [Fact]
        public void GetPayments_UsesParameterForCustomerNumber()
        {
            // Delta regression test: customerNumber filter changed from concatenation to @customerNumber parameter.
            var asmText = Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(typeof(SqliteDbProvider).Assembly.Location));
            Assert.Contains("customerNumber = @customerNumber", asmText);
        }
    }
}
