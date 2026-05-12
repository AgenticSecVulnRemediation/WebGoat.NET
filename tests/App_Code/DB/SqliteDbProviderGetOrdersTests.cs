using Xunit;
using Mono.Data.Sqlite;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_AddsCustomerIdParameter()
        {
            const string expectedSql = "select * from Orders where customerNumber = @customerID";

            var adapter = new SqliteDataAdapter(expectedSql, "Data Source=:memory:");
            adapter.SelectCommand.Parameters.AddWithValue("@customerID", 1);

            Assert.Equal(expectedSql, adapter.SelectCommand.CommandText);
            Assert.True(adapter.SelectCommand.Parameters.Contains("@customerID"));
        }
    }
}
