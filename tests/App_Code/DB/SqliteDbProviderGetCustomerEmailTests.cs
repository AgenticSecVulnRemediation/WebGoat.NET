using Xunit;
using System.IO;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterForCustomerNumber()
        {
            // Delta behavior: changed to parameterized query.
            var path = Path.Combine("WebGoat", "App_Code", "DB", "SqliteDbProvider.cs");
            var content = File.ReadAllText(path);

            Assert.Contains("where customerNumber = @customerNumber", content);
            Assert.Contains("Parameters.AddWithValue(\"@customerNumber\"", content);
            Assert.DoesNotContain("where customerNumber = \" + customerNumber", content);
        }
    }
}
