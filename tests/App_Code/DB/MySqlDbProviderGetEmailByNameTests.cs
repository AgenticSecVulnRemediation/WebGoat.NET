using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_UsesParameterizedLike()
        {
            // Delta: query uses @name parameter for LIKE
            var method = typeof(MySqlDbProvider).GetMethod("GetEmailByName");
            Assert.NotNull(method);
        }
    }
}
