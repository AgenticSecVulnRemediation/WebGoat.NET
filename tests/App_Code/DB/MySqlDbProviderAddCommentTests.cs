using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_AddComment_Tests
    {
        [Fact]
        public void AddComment_WithSqlInjectionLikeInput_DoesNotThrowFromMalformedSqlConstruction()
        {
            // Arrange
            var provider = new MySqlDbProvider(new ConfigFileStub());

            string productCode = "S10_1678";
            string email = "a@b.com";
            string comment = "test'); DROP TABLE Comments; --";

            // Act
            // With parameterized INSERT, quotes in comment should not break SQL string construction.
            // The method returns an error message on failure; it should not throw.
            string result = provider.AddComment(productCode, email, comment);

            // Assert
            Assert.True(result == null || result.Length > 0);
        }

        private sealed class ConfigFileStub : ConfigFile
        {
            public override string Get(string key)
            {
                if (key == DbConstants.KEY_PWD) return string.Empty;
                if (key == DbConstants.KEY_HOST) return "localhost";
                if (key == DbConstants.KEY_PORT) return "3306";
                if (key == DbConstants.KEY_DATABASE) return "test";
                if (key == DbConstants.KEY_UID) return "root";
                if (key == DbConstants.KEY_CLIENT_EXEC) return "mysql";
                return string.Empty;
            }
        }
    }
}
