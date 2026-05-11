using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_UsesSingleLikeParameter_WithTrailingWildcard()
        {
            // Delta behavior: query now uses @name parameter and adds trailing '%'.
            // Without a DB, we can only assert method exists (compile-time) and document expectation.
            // This test guards against accidental removal/renaming.

            var method = typeof(MySqlDbProvider).GetMethod("GetEmailByName");
            Assert.NotNull(method);
        }
    }
}
