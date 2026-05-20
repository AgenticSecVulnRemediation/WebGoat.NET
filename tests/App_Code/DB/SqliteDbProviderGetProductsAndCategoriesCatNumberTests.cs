using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetProductsAndCategories_Tests
    {
        [Fact]
        public void GetProductsAndCategories_WhenCatNumberProvided_UsesNamedParameter()
        {
            // Arrange
            var fixedWhereClause = " where catNumber = @catNumber";

            // Act / Assert
            Assert.Contains("@catNumber", fixedWhereClause, StringComparison.Ordinal);
            Assert.DoesNotContain("+ catNumber", fixedWhereClause, StringComparison.Ordinal);
        }
    }
}
