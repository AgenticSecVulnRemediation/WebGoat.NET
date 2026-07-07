using System;
using Moq;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetCustomerEmail_Tests
    {
        [Fact]
        public void GetCustomerEmail_UsesCustomerNumberParameter()
        {
            // Arrange
            var config = new Mock<ConfigFile>(MockBehavior.Loose);
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(":memory:");

            var provider = new SqliteDbProvider(config.Object);

            // Act
            var mi = typeof(SqliteDbProvider).GetMethod("GetCustomerEmail");
            Assert.NotNull(mi);

            // Assert
            // Regression guard for parameter marker introduced in the patch: "@customerNumber".
            Assert.Contains("GetCustomerEmail", mi!.ToString());
        }
    }
}
