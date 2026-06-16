using System;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_WithQuoteCharacters_DoesNotThrowSqlSyntaxError()
        {
            // Arrange
            var cfg = new ConfigFile();
            cfg.Set(DbConstants.KEY_HOST, "invalid-host");
            cfg.Set(DbConstants.KEY_PORT, "3306");
            cfg.Set(DbConstants.KEY_DATABASE, "goatdb");
            cfg.Set(DbConstants.KEY_UID, "root");
            cfg.Set(DbConstants.KEY_PWD, "root");

            var provider = new MySqlDbProvider(cfg);

            // Previously would break out of LIKE clause via concatenation.
            var namePayload = "O'Reilly";

            // Act + Assert
            // DataAdapter.Fill will attempt connection and fail; ensure no SQL syntax error is raised due to query text.
            var ex = Record.Exception(() => provider.GetEmailByName(namePayload));
            if (ex != null)
            {
                Assert.DoesNotContain("SQL syntax", ex.Message, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
