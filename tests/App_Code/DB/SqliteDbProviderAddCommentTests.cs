using System;
using System.Reflection;
using Moq;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameters_InInsert()
        {
            // Arrange
            var config = new Mock<ConfigFile>(MockBehavior.Loose);
            config.Setup(c => c.Get(It.IsAny<string>())).Returns("dummy");
            var method = typeof(SqliteDbProvider).GetMethod("AddComment");

            // Assert
            Assert.NotNull(method);
            var il = method!.GetMethodBody()!.GetILAsByteArray();
            var marker = System.Text.Encoding.UTF8.GetBytes("@Comment");
            Assert.Contains(marker, new ReadOnlySpan<byte>(il).ToArray());
        }
    }
}
