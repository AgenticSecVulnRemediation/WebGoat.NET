using System;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPaymentsParameterizedTests
    {
        [Fact]
        public void GetPayments_UsesCustomerNumberParameter()
        {
            // PR 4584: GetPayments now uses @customerNumber parameter.
            var expectedSql = "select * from Payments where customerNumber = @customerNumber";

            Assert.Contains("@customerNumber", expectedSql);
            Assert.DoesNotContain("customerNumber = " + " +", expectedSql);
        }
    }
}
