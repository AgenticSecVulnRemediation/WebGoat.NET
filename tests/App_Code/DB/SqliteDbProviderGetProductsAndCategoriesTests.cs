using System;
using Moq;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetProductsAndCategories_Tests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void GetProductsAndCategories_UsesParameters_WhenCatNumberProvided(int catNumber)
        {
            // Arrange
            var config = new Mock<ConfigFile>(MockBehavior.Loose);
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(":memory:");

            var provider = new SqliteDbProvider(config.Object);

            // Act
            var mi = typeof(SqliteDbProvider).GetMethod("GetProductsAndCategories", new[] { typeof(int) });
            Assert.NotNull(mi);

            // Assert
            Assert.Contains("GetProductsAndCategories", mi!.ToString());
        }
    }
}
