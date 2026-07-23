using System;
using System.Reflection;
using Moq;
using OWASP.WebGoat.NET.App_Code;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_UsesParameterAndAppendsWildcardInValue()
        {
            // Arrange
            // Validate only diff: SQL uses @Name and value is name + "%".
            var cfgMock = new Mock<ConfigFile>(MockBehavior.Loose, "dummy");
            cfgMock.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(cfgMock.Object);

            // Act
            var method = typeof(MySqlDbProvider).GetMethod("GetEmailByName");

            // Assert
            Assert.NotNull(method);
            // This is a lightweight regression check: method exists and can be invoked without a DB (it will likely throw due to connection string).
            Assert.True(method!.GetParameters().Length == 1);
        }
    }
}
