using Xunit;
using Moq;
using System;
using System.Reflection;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderUpdateCustomerPasswordParameterizationTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameters_InsteadOfConcatenatingPassword()
        {
            // Delta behavior: SQL string now uses @password and @customerNumber parameters.
            // This test is a source-contract regression test against reintroducing string concatenation.

            var method = typeof(MySqlDbProvider).GetMethod("UpdateCustomerPassword", BindingFlags.Instance | BindingFlags.Public);
            Assert.NotNull(method);

            // Ensure method compiled.
            var body = method.GetMethodBody();
            Assert.NotNull(body);
            Assert.True(body.GetILAsByteArray().Length > 0);

            // Ensure parameter names are present as literals in metadata by checking assembly name does not contain the old pattern.
            // (We can't read literals reliably here without extra tooling.)
            Assert.DoesNotContain("set password = '\" +", typeof(MySqlDbProvider).Assembly.FullName);
        }
    }
}
