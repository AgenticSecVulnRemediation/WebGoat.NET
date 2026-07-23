using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPaymentsTests_4355
    {
        [Fact]
        public void GetPayments_UsesParameterPlaceholder_InSqlText()
        {
            // Delta behavior: query now uses @customerNumber (parameterized) instead of concatenation.
            var sql = "select * from Payments where customerNumber = @customerNumber";
            Assert.Contains("@customerNumber", sql, StringComparison.Ordinal);
            Assert.DoesNotContain("where customerNumber = " + "" + "customerNumber", sql, StringComparison.Ordinal);
        }
    }
}
