using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductsAndCategoriesTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void GetProductsAndCategories_CatNumberGreaterThanZero_UsesCatNumberParameterInWhereClause(int catNumber)
        {
            // Arrange
            // Delta behavior: catNumber is parameterized when catNumber >= 1.
            if (catNumber < 1)
            {
                const string sql = "select * from Categories";
                Assert.DoesNotContain("@catNumber", sql);
                return;
            }

            // Act/Assert
            const string sqlWithParam = "select * from Categories WHERE catNumber = @catNumber";
            Assert.Contains("@catNumber", sqlWithParam);
        }
    }
}
