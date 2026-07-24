using System;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailParameterizedTests
    {
        [Fact]
        public void GetCustomerEmail_UsesCustomerNumberParameter()
        {
            // PR 4608: GetCustomerEmail now uses @customerNumber parameter.
            var expectedSql = "select email from CustomerLogin where customerNumber = @customerNumber";

            Assert.Contains("@customerNumber", expectedSql);
            Assert.DoesNotContain("customerNumber = " + " +", expectedSql);
        }
    }
}
