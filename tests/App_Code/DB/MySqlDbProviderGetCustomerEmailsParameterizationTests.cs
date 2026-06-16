using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailsParameterizationTests
    {
        [Fact]
        public void GetCustomerEmails_UsesParameterizedLikeQuery_AppendsPercentInParameterValue()
        {
            // Arrange
            // Fix changed SQL from "... like '" + email + "%'" to "... like @email" and adds email + "%".
            var method = typeof(MySqlDbProvider).GetMethod("GetCustomerEmails");
            Assert.NotNull(method);

            // Act/Assert (deterministic, delta-focused)
            var asm = typeof(MySqlDbProvider).Assembly.ToString();
            Assert.Contains("like @email", asm);
            Assert.Contains("email + \"%\"", asm);
        }
    }
}
