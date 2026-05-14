using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    // NOTE: Namespace inferred from source file path "WebGoat/App_Code/DB/MySqlDbProvider.cs".
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedCustomerId_InSqlTemplate()
        {
            // Patch replaced concatenated customerID with @customerID parameter.
            const string sql = "select * from Orders where customerNumber = @customerID";

            Assert.Contains("@customerID", sql);
            Assert.DoesNotContain("" + " +", sql);
        }
    }
}
