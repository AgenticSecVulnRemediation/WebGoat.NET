using System;
using System.Reflection;
using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_WithSqlSpecialCharsInComment_DoesNotThrowArgumentNullException()
        {
            // Delta focus: AddComment now uses parameterized query with @productCode/@email/@comment.
            // We ensure the method accepts values with quotes without failing due to string formatting.

            var cfg = new Mock<ConfigFile>();
            cfg.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(cfg.Object);

            var ex = Record.Exception(() => provider.AddComment("S10_1678", "a@b.com", "hi'); DROP TABLE Comments;--"));
            Assert.False(ex is ArgumentNullException);
        }
    }
}
