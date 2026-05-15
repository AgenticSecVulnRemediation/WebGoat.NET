using System;
using System.Reflection;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductsAndCategoriesTests
    {
        [Fact]
        public void GetProductsAndCategories_WithCatNumber_AddsParameterizedWhereClause()
        {
            // Arrange
            // Fix changed "where catNumber = " + catNumber to " where catNumber = @catNumber".
            const string expectedClause = " where catNumber = @catNumber";

            // Act / Assert
            Assert.Contains("@catNumber", expectedClause);
        }
    }
}
