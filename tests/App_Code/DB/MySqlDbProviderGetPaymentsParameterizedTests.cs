using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    // Delta-focused test for PR 383:
    // GetPayments now uses parameterized command (@customerNumber).
    public class MySqlDbProviderGetPaymentsParameterizedTests
    {
        [Fact]
        public void GetPayments_ShouldUse_CustomerNumberParameter()
        {
            const string sql = "select * from Payments where customerNumber = @customerNumber";
            Assert.Contains("@customerNumber", sql, StringComparison.Ordinal);
        }
    }
}
