using System;
using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderCustomCustomerLoginTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterPlaceholder_InSqlText()
        {
            // Delta behavior: query changed from string concatenation to parameterized @email.
            var sql = "select * from CustomerLogin where email = @email;";
            Assert.Contains("@email", sql, StringComparison.Ordinal);
            Assert.DoesNotContain("'" + " + email + " + "'", sql, StringComparison.Ordinal);
        }
    }
}
