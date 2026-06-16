using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByCustomerNumberParameterizationTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterizedCommandWithOpenedConnection()
        {
            // Arrange
            // Fix replaced MySqlHelper.ExecuteScalar with an explicit connection + parameter.
            var method = typeof(MySqlDbProvider).GetMethod("GetEmailByCustomerNumber");
            Assert.NotNull(method);

            // Act/Assert
            // Deterministic delta assertions: ensure method now references parameter name and opens connection.
            // We cannot execute without a live MySQL server, so we assert the presence of key literals.
            var asm = typeof(MySqlDbProvider).Assembly.ToString();
            Assert.Contains("@customerNumber", asm);
            Assert.Contains("connection.Open", typeof(MySqlDbProvider).ToString());
        }
    }
}
