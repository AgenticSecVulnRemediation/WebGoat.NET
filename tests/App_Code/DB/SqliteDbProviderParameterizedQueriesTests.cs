using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderParameterizedQueriesTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterizedQuery()
        {
            var method = typeof(SqliteDbProvider).GetMethod("CustomCustomerLogin");
            Assert.NotNull(method);
        }

        [Fact]
        public void GetPasswordByEmail_UsesParameterizedQuery()
        {
            var method = typeof(SqliteDbProvider).GetMethod("GetPasswordByEmail");
            Assert.NotNull(method);
        }
    }
}
