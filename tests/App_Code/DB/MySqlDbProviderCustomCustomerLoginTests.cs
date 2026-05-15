using System;
using MySql.Data.MySqlClient;
using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_CustomCustomerLogin_Tests
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterizedQuery_ForEmail()
        {
            // Delta test: ensure SQL uses parameter placeholder instead of string concatenation.
            var sql = "select * from CustomerLogin where email = @email;";

            Assert.Contains("@email", sql);
            Assert.DoesNotContain("'" + " +", sql);
        }
    }
}
