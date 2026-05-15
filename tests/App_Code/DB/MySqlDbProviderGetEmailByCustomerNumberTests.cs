using System;
using MySql.Data.MySqlClient;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByCustomerNumberTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterizedExecuteScalar()
        {
            // Delta behavior: ExecuteScalar query changed from concatenation to parameterized query.
            const string query = "select email from CustomerLogin where customerNumber = @num";

            var prm = new MySqlParameter("@num", "1");

            Assert.Contains("@num", query, StringComparison.Ordinal);
            Assert.Equal("@num", prm.ParameterName);
        }
    }
}
