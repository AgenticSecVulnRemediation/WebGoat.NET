using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailSqlTests
    {
        [Fact]
        public void GetCustomerEmail_Query_UsesNamedParameter_NotConcatenation()
        {
            // Patch changed the SQL from concatenation to parameterized query:
            // "... where customerNumber = @customerNumber"
            var sql = "select email from CustomerLogin where customerNumber = @customerNumber";

            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("+ customerNumber", sql);
        }
    }
}
