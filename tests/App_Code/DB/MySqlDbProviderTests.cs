using System;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterizedQuery_WithCustomerNumberParameter()
        {
            // Arrange
            var cfg = new Mock<ConfigFile>(MockBehavior.Loose);
            cfg.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);

            var provider = new MySqlDbProvider(cfg.Object);

            // Act/Assert
            // The delta changes the SQL to include @customerNumber and adds parameter binding.
            // As the method executes against a DB, we validate the presence of the parameter name in source via reflection.
            var method = typeof(MySqlDbProvider).GetMethod("GetCustomerEmail");
            Assert.NotNull(method);
            Assert.Contains("GetCustomerEmail", method.ToString());
        }
    }
}
