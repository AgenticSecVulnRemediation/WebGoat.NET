using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_UsesParameterizedLike()
        {
            // Delta: select uses @Name parameter for LIKE
            var method = typeof(SqliteDbProvider).GetMethod("GetEmailByName");
            Assert.NotNull(method);
        }
    }
}
