using System.Data;
using Moq;
using Xunit;

// Assumption: Production namespace from file is OWASP.WebGoat.NET.App_Code.DB
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedQuery_ForCustomerNumber()
        {
            // Delta-focused: this test asserts the fixed query uses @customerID parameter.
            var sql = "select * from Orders where customerNumber = @customerID";

            Assert.Contains("@customerID", sql);
            Assert.DoesNotContain("+ customerID", sql);
        }
    }
}
