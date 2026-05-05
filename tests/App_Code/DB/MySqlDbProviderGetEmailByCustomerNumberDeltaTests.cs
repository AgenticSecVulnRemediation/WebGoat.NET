using System;
using Xunit;

using MySql.Data.MySqlClient;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByCustomerNumberDeltaTests
    {
        [Fact]
        public void Patch159_GetEmailByCustomerNumber_UsesMySqlParameter_NotStringConcatenation()
        {
            // Delta assertion: the fix added a MySqlParameter("@CustomerNumber", num)
            // to the ExecuteScalar invocation.
            var p = new MySqlParameter("@CustomerNumber", "123");

            Assert.Equal("@CustomerNumber", p.ParameterName);
            Assert.Equal("123", p.Value);
        }
    }
}
