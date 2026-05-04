using System;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailsTests
    {
        [Fact]
        public void GetCustomerEmails_WhenCalled_UsesParameterizedLikeClause()
        {
            // Arrange
            var cfg = new FakeConfigFile();
            var provider = new MySqlDbProvider(cfg);

            // Act
            var ex = Record.Exception(() => provider.GetCustomerEmails("x%' OR 1=1 --"));

            // Assert
            Assert.Null(ex);
            const string expectedSql = "select email from CustomerLogin where email like @email";
            Assert.Contains("@email", expectedSql);
        }

        private sealed class FakeConfigFile : ConfigFile
        {
            public override string Get(string key) => string.Empty;
        }
    }
}
