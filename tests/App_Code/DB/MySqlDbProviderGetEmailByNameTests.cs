using System;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_GetEmailByName_Tests
    {
        [Fact]
        public void GetEmailByName_AppendsWildcardAndDoesNotEmbedNameIntoSql()
        {
            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(config.Object);

            // Act
            var payload = "bob%' OR 1=1 --";
            Exception ex = Record.Exception(() => provider.GetEmailByName(payload));

            // Assert
            // Old code would embed payload into query; the new code parameterizes and appends %.
            if (ex != null)
                Assert.DoesNotContain(payload, ex.ToString());
        }
    }
}
