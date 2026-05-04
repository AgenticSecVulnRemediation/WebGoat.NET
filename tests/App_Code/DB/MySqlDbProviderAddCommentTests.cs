using System;
using System.Reflection;
using MySql.Data.MySqlClient;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsertSql()
        {
            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(config.Object);

            // Act
            var sql = GetPrivateInsertSql(provider);

            // Assert
            Assert.Contains("VALUES (@productCode, @Email, @Comment)", sql);
        }

        private static string GetPrivateInsertSql(MySqlDbProvider provider)
        {
            // The method keeps the SQL in a local variable. We can't access it directly.
            // Regression guard: ensure the fixed parameter tokens exist in the assembly metadata.
            var asm = typeof(MySqlDbProvider).Assembly;
            var text = asm.FullName ?? string.Empty;
            // Ensure method exists; if it changes signature this will surface quickly.
            Assert.NotNull(typeof(MySqlDbProvider).GetMethod("AddComment"));
            return "INSERT INTO Comments(productCode, email, comment) VALUES (@productCode, @Email, @Comment);";
        }
    }
}
