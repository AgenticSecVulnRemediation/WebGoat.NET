using System;
using System.Collections.Specialized;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_UsesLikeParameterWithTrailingWildcard_AcceptsQuotes()
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
            // Parameterization means names with quotes should not break SQL string construction.
            var ex = Record.Exception(() => provider.GetEmailByName("O'Reilly"));
            Assert.Null(ex);
        }
    }
}
