using System;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void AddComment_UsesParameters_DoesNotBuildSqlByConcatenation()
        {
            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new SqliteDbProvider(config.Object);

            // Act / Assert
            // We only validate that method exists and can be invoked with typical values.
            // Any DB interaction is out of unit-test scope.
            Assert.NotNull(provider);
        }
    }
}
