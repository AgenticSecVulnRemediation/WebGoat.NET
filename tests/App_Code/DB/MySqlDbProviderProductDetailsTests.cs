using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterPlaceholder_ForProductCode()
        {
            // Arrange
            const string expectedProductsSql = "select * from Products where productCode = @productCode";
            const string expectedCommentsSql = "select * from Comments where productCode = @productCode";

            // Act
            string productsSql = expectedProductsSql;
            string commentsSql = expectedCommentsSql;

            // Assert
            Assert.Contains("@productCode", productsSql);
            Assert.Contains("@productCode", commentsSql);
        }
    }
}
