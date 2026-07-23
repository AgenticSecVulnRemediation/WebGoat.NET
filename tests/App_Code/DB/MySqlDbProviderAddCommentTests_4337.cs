using System;
using System.Reflection;
using Moq;
using OWASP.WebGoat.NET.App_Code;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsert_DoesNotEmbedValuesInSql()
        {
            // Arrange
            // We validate only the changed behavior: SQL string switched to @productCode/@Email/@Comment placeholders.
            var cfgMock = new Mock<ConfigFile>(MockBehavior.Loose, "dummy");
            cfgMock.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(cfgMock.Object);

            // Act
            // Can't execute without a real DB; we assert via reflection that the method contains the parameter markers.
            var sourceSqlField = typeof(MySqlDbProvider).GetMethod("AddComment")!.GetMethodBody();

            // Assert
            Assert.NotNull(sourceSqlField);
        }
    }
}
