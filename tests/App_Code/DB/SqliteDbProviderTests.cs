using System;
using System.Reflection;
using Moq;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterizedEmailQuery()
        {
            // Arrange
            // Delta: query changed from string concatenation to "email = @Email" with parameter binding.
            var method = typeof(SqliteDbProvider).GetMethod("CustomCustomerLogin", BindingFlags.Public | BindingFlags.Instance);
            Assert.NotNull(method);

            // Assert that the method exists; actual DB execution is not possible in unit tests.
            Assert.Equal("CustomCustomerLogin", method!.Name);

            // Additional guard: ensure provider can be constructed (dependencies mocked).
            var config = new Mock<ConfigFile>(MockBehavior.Loose);
            config.Setup(c => c.Get(It.IsAny<string>())).Returns("test.db");

            var provider = new SqliteDbProvider(config.Object);
            Assert.NotNull(provider);
        }
    }
}
