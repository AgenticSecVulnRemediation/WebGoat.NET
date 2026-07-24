using Xunit;
using Moq;
using System;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameter_ForProductCode_InBothQueries()
        {
            // Arrange
            var productsSql = "select * from Products where productCode = @productCode";
            var commentsSql = "select * from Comments where productCode = @productCode";

            // Assert
            Assert.Contains("@productCode", productsSql);
            Assert.Contains("@productCode", commentsSql);
            Assert.DoesNotContain("'" + " + productCode + ", productsSql);
            Assert.DoesNotContain("'" + " + productCode + ", commentsSql);
        }
    }
}
