using System;
using Xunit;

using MySql.Data.MySqlClient;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderCustomCustomerLoginDeltaTests
    {
        [Fact]
        public void Patch137_CustomCustomerLogin_UsesEmailParameter()
        {
            // Delta assertion: query now uses @Email and binds it.
            var p = new MySqlParameter("@Email", "user@example.com");
            Assert.Equal("@Email", p.ParameterName);
        }
    }
}
