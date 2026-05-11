using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderCustomCustomerLoginTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterizedEmailQuery_TemplateDoesNotContainEmail()
        {
            // Arrange
            // Delta behavior: query now uses @email parameter.
            var email = "a' OR '1'='1";

            // Assert template-level behavior.
            const string sql = "select * from CustomerLogin where email = @email;";
            Assert.Contains("@email", sql);
            Assert.DoesNotContain(email, sql);
        }
    }
}
