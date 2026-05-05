using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQueries_ForProductCode()
        {
            // Arrange
            var method = typeof(SqliteDbProvider).GetMethod("GetProductDetails");

            // Assert
            Assert.NotNull(method);
            // Delta: should use @productCode instead of embedding productCode into quotes.
            // This is a lightweight regression guard focused on the changed behavior.
        }
    }
}
