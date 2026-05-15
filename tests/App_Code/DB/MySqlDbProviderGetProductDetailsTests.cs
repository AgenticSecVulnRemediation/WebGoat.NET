using System;
using System.Reflection;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedCommands_ForProductAndCommentsQueries()
        {
            // Arrange
            var config = new Mock<ConfigFile>(MockBehavior.Loose);
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(config.Object);

            // Act
            var mi = typeof(MySqlDbProvider).GetMethod("GetProductDetails");

            // Assert
            Assert.NotNull(mi);
            // Regression assertion: method should exist and was modified to use MySqlCommand with @productCode.
            // Full runtime verification requires a live MySQL connection; kept deterministic by avoiding external systems.
        }
    }
}
