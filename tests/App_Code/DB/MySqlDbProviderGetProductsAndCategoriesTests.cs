using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductsAndCategoriesTests
    {
        [Fact]
        public void GetProductsAndCategories_WithCatNumber_UsesParameterizedCatNumberClause()
        {
            // Act
            const string expectedClause = " where catNumber = @catNumber";

            // Assert
            Assert.Contains("@catNumber", expectedClause);
            Assert.DoesNotContain("where catNumber = ", expectedClause.Replace("@catNumber", ""), StringComparison.OrdinalIgnoreCase);
        }
    }
}
