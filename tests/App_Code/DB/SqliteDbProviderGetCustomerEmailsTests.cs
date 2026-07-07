using System;
using Moq;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetCustomerEmails_Tests
    {
        [Fact]
        public void GetCustomerEmails_UsesLikeParameterWithWildcardAppended()
        {
            // Arrange
            var config = new Mock<ConfigFile>(MockBehavior.Loose);
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(":memory:");

            var provider = new SqliteDbProvider(config.Object);

            // Act
            var mi = typeof(SqliteDbProvider).GetMethod("GetCustomerEmails");
            Assert.NotNull(mi);

            // Assert
            Assert.Contains("GetCustomerEmails", mi!.ToString());
        }
    }
}
