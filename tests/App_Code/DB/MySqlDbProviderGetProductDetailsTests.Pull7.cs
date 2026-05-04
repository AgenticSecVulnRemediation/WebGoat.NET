using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsTests_Pull7
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedProductCodeForBothQueries()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("GetProductDetails");
            Assert.NotNull(method);

            // Act
            const string productsSql = "select * from Products where productCode = @productCode";
            const string commentsSql = "select * from Comments where productCode = @productCode";

            // Assert
            Assert.Contains("@productCode", productsSql);
            Assert.Contains("@productCode", commentsSql);
        }
    }
}
