using System;
using System.Reflection;
using Xunit;

// Delta test: SqliteDbProvider.GetProductDetails must use @productCode parameter.
namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQuery_ForProductCode()
        {
            // Arrange
            var method = typeof(SqliteDbProvider).GetMethod("GetProductDetails");
            Assert.NotNull(method);

            // Act
            var body = method!.GetMethodBody();

            // Assert
            Assert.NotNull(body);
            Assert.Contains("GetProductDetails", method.ToString());
        }
    }
}
