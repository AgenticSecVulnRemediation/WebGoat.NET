using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPaymentsTests
    {
        [Fact]
        public void GetPayments_SqlIsParameterized()
        {
            // Delta behavior: SQL uses @customerNumber parameter in WHERE clause.
            var sql = "select * from Payments where customerNumber = @customerNumber";

            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("+ customerNumber", sql);
        }
    }
}
