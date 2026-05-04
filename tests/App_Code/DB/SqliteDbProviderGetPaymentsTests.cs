using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPaymentsTests
    {
        [Fact]
        public void GetPayments_UsesParameterizedCustomerNumber()
        {
            const string expectedSql = "select * from Payments where customerNumber = @customerNumber";
            Assert.Contains("@customerNumber", expectedSql);
            Assert.DoesNotContain("customerNumber = ", expectedSql.Replace("@customerNumber", ""), StringComparison.OrdinalIgnoreCase);
        }
    }
}
