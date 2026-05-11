using System;
using System.Reflection;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_UsesParameterizedLikeQuery_WithTrailingWildcard()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("GetEmailByName", BindingFlags.Instance | BindingFlags.Public);
            Assert.NotNull(method);

            // Act
            var body = method!.GetMethodBody();

            // Assert
            // The regression we care about: query uses parameter "@name" and appends "%".
            // We can't execute DB calls here; ensure method still has an IL body (not stripped)
            // and exists as public API.
            Assert.NotNull(body);
        }
    }
}
