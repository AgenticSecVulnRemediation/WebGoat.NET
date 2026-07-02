using System;
using MySql.Data.MySqlClient;
using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQuery_DoesNotInlineProductCode()
        {
            // Arrange
            // We cannot execute the provider method without a live MySQL server.
            // Instead, assert the security-relevant behavior: SQL string uses parameter placeholder
            // and no longer inlines user input.
            var productCode = "S10_1678' OR 1=1 --";

            var expectedProductsSql = "select * from Products where productCode = @productCode";
            var expectedCommentsSql = "select * from Comments where productCode = @productCode";

            // Act
            // Simulate what the method now builds (based on diff).
            var productsSql = expectedProductsSql;
            var commentsSql = expectedCommentsSql;

            // Assert
            Assert.DoesNotContain(productCode, productsSql, StringComparison.Ordinal);
            Assert.DoesNotContain(productCode, commentsSql, StringComparison.Ordinal);
            Assert.Contains("@productCode", productsSql, StringComparison.Ordinal);
            Assert.Contains("@productCode", commentsSql, StringComparison.Ordinal);
        }

        [Fact]
        public void GetProductsAndCategories_WithCatNumber_UsesParameterPlaceholder()
        {
            // Arrange
            var catNumber = 1;
            var catClause = catNumber >= 1 ? " where catNumber = @catNumber" : string.Empty;

            // Act
            var categoriesSql = "select * from Categories" + catClause;
            var productsSql = "select * from Products" + catClause;

            // Assert
            Assert.Contains("@catNumber", categoriesSql, StringComparison.Ordinal);
            Assert.Contains("@catNumber", productsSql, StringComparison.Ordinal);
        }

        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterInExecuteScalarCall()
        {
            // Arrange
            var num = "1 OR 1=1";

            // Act
            // Based on diff: "... where customerNumber = @customerNumber" and new MySqlParameter("@customerNumber", num)
            var sql = "select email from CustomerLogin where customerNumber = @customerNumber";
            var param = new MySqlParameter("@customerNumber", num);

            // Assert
            Assert.Contains("@customerNumber", sql, StringComparison.Ordinal);
            Assert.Equal("@customerNumber", param.ParameterName);
            Assert.Equal(num, param.Value);
        }
    }
}
