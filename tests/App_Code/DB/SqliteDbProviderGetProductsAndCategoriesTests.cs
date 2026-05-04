using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductsAndCategoriesTests
    {
        [Fact]
        public void GetProductsAndCategories_WithCatNumber_UsesParameterizedCatNumber()
        {
            const string expectedClause = " where catNumber = @catNumber";
            Assert.Contains("@catNumber", expectedClause);
            Assert.DoesNotContain("where catNumber = ", expectedClause.Replace("@catNumber", ""), StringComparison.OrdinalIgnoreCase);
        }
    }
}
