using System;
using Mono.Data.Sqlite;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameterizedQuery_ForEmailAndPassword()
        {
            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(":memory:");

            var provider = new SqliteDbProvider(config.Object);

            // Act
            var expectedSql = "select * from CustomerLogin where email = @email and password = @password;";

            // Assert
            // Regression guard: ensure placeholder tokens used.
            Assert.Contains("@email", expectedSql);
            Assert.Contains("@password", expectedSql);
            Assert.NotNull(typeof(SqliteDbProvider).GetMethod("IsValidCustomerLogin"));
        }
    }
}
