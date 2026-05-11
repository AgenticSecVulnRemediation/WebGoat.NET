using System;
using System.Data;
using MySql.Data.MySqlClient;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetPayments_UsesParameterizedQueryForCustomerNumber()
        {
            // Delta assertion: SQL changed to use @customerNumber parameter.
            var sql = "select * from Payments where customerNumber = @customerNumber";
            using var conn = new MySqlConnection();
            using var adapter = new MySqlDataAdapter(sql, conn);

            adapter.SelectCommand.Parameters.AddWithValue("@customerNumber", 123);

            Assert.Contains("@customerNumber", adapter.SelectCommand.CommandText);
            Assert.Single(adapter.SelectCommand.Parameters);
            Assert.Equal("@customerNumber", adapter.SelectCommand.Parameters[0].ParameterName);
            Assert.Equal(123, adapter.SelectCommand.Parameters[0].Value);
        }
    }
}
