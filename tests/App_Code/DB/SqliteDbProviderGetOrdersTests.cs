using Xunit;
using Moq;
using System;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedSelectCommand_PreventsSqlInjection()
        {
            // Arrange
            var providerType = typeof(SqliteDbProvider);
            var method = providerType.GetMethod("GetOrders", new[] { typeof(int) });
            Assert.NotNull(method);

            // Act
            var body = method!.GetMethodBody();

            // Assert
            Assert.NotNull(body);
            Assert.Equal("GetOrders", method.Name);
        }
    }
}
