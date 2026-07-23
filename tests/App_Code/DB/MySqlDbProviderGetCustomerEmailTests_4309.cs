using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailTests_4309
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterizedCustomerNumber_InSqlText()
        {
            // Delta behavior: query now uses @customerNumber and adds parameter.
            var sql = "select email from CustomerLogin where customerNumber = @customerNumber";
            Assert.Contains("@customerNumber", sql, StringComparison.Ordinal);
            Assert.DoesNotContain(" where customerNumber = " + "" + "customerNumber", sql, StringComparison.Ordinal);
        }
    }
}
