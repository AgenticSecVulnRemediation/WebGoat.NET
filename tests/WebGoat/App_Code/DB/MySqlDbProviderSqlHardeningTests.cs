using MySql.Data.MySqlClient;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderSqlHardeningTests
    {
        [Fact]
        public void GetOrders_SqlIsParameterized_ContainsCustomerIdParameter()
        {
            // Delta: changed from string concatenation to parameterized query.
            const string sql = "select * from Orders where customerNumber = @customerID";
            var cmd = new MySqlCommand(sql);
            cmd.Parameters.AddWithValue("@customerID", 123);

            Assert.Equal(sql, cmd.CommandText);
            Assert.True(cmd.Parameters.Contains("@customerID"));
            Assert.Equal(123, cmd.Parameters["@customerID"].Value);
        }

        [Fact]
        public void GetProductDetails_SqlIsParameterized_ProductCodeNotInterpolated()
        {
            // Delta: changed from quoted concatenation to parameterized query.
            const string sql = "select * from Products where productCode = @productCode";
            string injected = "S10_1678' OR 1=1 --";

            var cmd = new MySqlCommand(sql);
            cmd.Parameters.AddWithValue("@productCode", injected);

            Assert.Equal(sql, cmd.CommandText);
            Assert.True(cmd.Parameters.Contains("@productCode"));
            Assert.Equal(injected, cmd.Parameters["@productCode"].Value);
            Assert.DoesNotContain(injected, cmd.CommandText);
        }

        [Fact]
        public void GetEmailByName_SqlIsParameterized_LikeWildcardIsInParameterValue()
        {
            // Delta: changed from LIKE concatenation to parameterized LIKE.
            const string sql = "select firstName, lastName, email from Employees where firstName like @name or lastName like @name";
            string injected = "rob' OR 1=1 --";

            var cmd = new MySqlCommand(sql);
            cmd.Parameters.AddWithValue("@name", injected + "%");

            Assert.Equal(sql, cmd.CommandText);
            Assert.True(cmd.Parameters.Contains("@name"));
            Assert.Equal(injected + "%", cmd.Parameters["@name"].Value);
            Assert.DoesNotContain(injected, cmd.CommandText);
        }
    }
}
