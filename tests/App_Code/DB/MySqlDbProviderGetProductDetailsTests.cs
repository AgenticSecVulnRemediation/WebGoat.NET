using System;
using System.Collections.Specialized;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_WithInjectionLikeInput_DoesNotThrowDuringAdapterSetup()
        {
            // Arrange
            var nvc = new NameValueCollection
            {
                [DbConstants.KEY_HOST] = "localhost",
                [DbConstants.KEY_PORT] = "3306",
                [DbConstants.KEY_DATABASE] = "db",
                [DbConstants.KEY_UID] = "u",
                [DbConstants.KEY_PWD] = "" ,
                [DbConstants.KEY_CLIENT_EXEC] = "mysql"
            };
            var provider = new MySqlDbProvider(new ConfigFile(nvc));

            // Act + Assert
            // We cannot connect to DB in unit tests; the regression we can assert is that the method can be invoked
            // without failing due to malformed SQL concatenation when special characters are present.
            // The parameterized implementation should accept arbitrary strings.
            var ex = Record.Exception(() => provider.GetProductDetails("abc' OR '1'='1"));
            Assert.Null(ex);
        }
    }
}
