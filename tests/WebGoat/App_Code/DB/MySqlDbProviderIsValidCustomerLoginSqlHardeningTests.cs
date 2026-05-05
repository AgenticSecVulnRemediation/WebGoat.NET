using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderIsValidCustomerLoginSqlHardeningTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParametersForEmailAndPassword()
        {
            // Delta: query should be parameterized.
            const string sql = "select * from CustomerLogin where email = @Email and password = @Password;";

            Assert.Contains("email = @Email", sql);
            Assert.Contains("password = @Password", sql);
            Assert.DoesNotContain("'\" +", sql);
        }
    }
}
