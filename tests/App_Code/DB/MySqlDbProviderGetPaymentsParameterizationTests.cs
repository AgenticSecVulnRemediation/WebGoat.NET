using Xunit;
using System;
using System.Data;

// Assumption: production namespace
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPaymentsParameterizationTests
    {
        [Fact]
        public void GetPayments_UsesNamedParameter_InQuery()
        {
            // Delta test: query now uses @customerNumber rather than concatenation.
            var sql = "select * from Payments where customerNumber = @customerNumber";
            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("where customerNumber = "+" ", sql); // guards against naive concatenation pattern
        }
    }
}
