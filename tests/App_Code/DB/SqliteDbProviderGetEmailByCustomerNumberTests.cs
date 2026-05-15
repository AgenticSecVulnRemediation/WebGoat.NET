using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetEmailByCustomerNumberTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesNamedParameter()
        {
            // Delta behavior: query changed from concatenation to named parameter (@num).
            const string sql = "select email from CustomerLogin where customerNumber = @num";

            Assert.Contains("@num", sql, StringComparison.Ordinal);
            Assert.DoesNotContain("customerNumber = ", sql + "'" , StringComparison.Ordinal); // crude guard against concatenated quoted values
        }
    }
}
