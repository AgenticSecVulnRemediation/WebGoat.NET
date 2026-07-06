using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameters_AllowsQuotesInComment()
        {
            // Arrange
            var configMock = new Mock<ConfigFile>(MockBehavior.Loose);
            configMock.Setup(c => c.Get(It.IsAny<string>())).Returns("dummy");
            var provider = new SqliteDbProvider(configMock.Object);

            // Act
            var ex = Record.Exception(() => provider.AddComment("S10_1678", "a@b.com", "x'); DROP TABLE Comments;--"));

            // Assert
            Assert.Null(ex);
        }
    }
}
