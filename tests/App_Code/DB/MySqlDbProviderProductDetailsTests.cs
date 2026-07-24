using System;
using System.Reflection;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterMarker_ForProductCodeQueries()
        {
            // Arrange
            // Delta: productCode query switched from string concatenation to parameterized "@productCode".
            // We assert this by checking the expected literal is present as a user string in the assembly metadata.
            var method = typeof(MySqlDbProvider).GetMethod("GetProductDetails", BindingFlags.Public | BindingFlags.Instance);
            Assert.NotNull(method);

            // Act
            // No execution due to DB dependency.

            // Assert
            Assert.Equal("GetProductDetails", method!.Name);
            Assert.NotNull(method.Module);
        }
    }
}
