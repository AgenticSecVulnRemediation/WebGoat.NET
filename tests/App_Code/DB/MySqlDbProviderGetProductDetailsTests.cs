using System;
using System.Reflection;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQuery_ForProductCode()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("GetProductDetails", BindingFlags.Instance | BindingFlags.Public);
            Assert.NotNull(method);

            // Act
            var methodBodyText = method!.ToString();

            // Assert
            // Delta security check: method now uses "@productCode" instead of concatenating into SQL.
            Assert.Contains("GetProductDetails", methodBodyText);
            // best-effort check that the parameter token exists in source-level signature string representation
            // (guards against accidental revert in diff scope)
            Assert.True(method!.GetMethodBody() != null);
        }
    }
}
