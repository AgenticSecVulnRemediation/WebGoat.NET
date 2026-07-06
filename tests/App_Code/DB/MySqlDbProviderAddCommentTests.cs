using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_AllowsSqlMetacharactersInInputs_WithoutFormatException()
        {
            // Arrange
            var configMock = new Mock<ConfigFile>(MockBehavior.Loose);
            configMock.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(configMock.Object);

            // Act
            var ex = Record.Exception(() => provider.AddComment("S10_1678", "a@b.com", "x'); DROP TABLE Comments;--"));

            // Assert
            // Post-fix uses parameters; it should not fail due to quote balancing while building SQL.
            Assert.Null(ex);
        }
    }
}
