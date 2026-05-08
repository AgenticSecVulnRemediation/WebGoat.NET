using System;
using System.Text;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetEmailByCustomerNumberParameterizationTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameter()
        {
            // Delta regression test: customerNumber lookup changed to use @num parameter.
            var asmText = Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(typeof(SqliteDbProvider).Assembly.Location));
            Assert.Contains("customerNumber = @num", asmText);
            Assert.Contains("Parameters.AddWithValue(\"@num\"", asmText);
        }
    }
}
