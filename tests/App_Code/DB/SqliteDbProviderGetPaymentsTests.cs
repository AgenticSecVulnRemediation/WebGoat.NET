using System.IO;
using Xunit;

// Source-level regression test for PR 3970: GetPayments now uses a parameter placeholder.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPaymentsTests
    {
        [Fact]
        public void GetPayments_UsesParameterizedCustomerNumberQuery()
        {
            var path = Path.Combine("WebGoat", "App_Code", "DB", "SqliteDbProvider.cs");
            var code = File.ReadAllText(path);

            Assert.Contains("select * from Payments where customerNumber = @customerNumber", code);
            Assert.Contains("Parameters.AddWithValue(\"@customerNumber\", customerNumber", code);
            Assert.Contains("new SqliteDataAdapter(cmd)", code);

            Assert.DoesNotContain("where customerNumber = \" + customerNumber", code);
        }
    }
}
