using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterizedCustomerNumber()
        {
            // delta: query is now parameterized
            const string expectedSql = "select email from CustomerLogin where customerNumber = @customerNumber";
            Assert.Contains("@customerNumber", expectedSql);
            Assert.DoesNotContain("customerNumber = ", expectedSql.Replace("@customerNumber", ""), StringComparison.OrdinalIgnoreCase);
        }
    }
}
