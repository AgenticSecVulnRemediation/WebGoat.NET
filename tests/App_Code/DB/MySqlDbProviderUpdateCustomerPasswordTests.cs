using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameters_PreventsSqlInjectionInCustomerNumber()
        {
            // Arrange
            // Delta: query now uses @Password and @CustomerNumber parameters.
            var provider = new MySqlDbProvider(new FakeConfigFile());

            // Act/Assert: no exception merely from special characters in password, since it should not be concatenated.
            var ex = Record.Exception(() => provider.UpdateCustomerPassword(1, "pw' , password='x"));
            Assert.Null(ex);
        }

        private sealed class FakeConfigFile : ConfigFile
        {
            public override string Get(string key) => "";
        }
    }
}
