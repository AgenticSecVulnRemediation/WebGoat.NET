using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductsAndCategoriesTests
    {
        [Fact]
        public void GetProductsAndCategories_WhenCategoryProvided_UsesParameter()
        {
            // Arrange
            var asmText = System.IO.File.ReadAllText(typeof(SqliteDbProvider).Assembly.Location);

            // Assert
            Assert.Contains("where catNumber = @catNum", asmText);
            Assert.Contains("cmd.Parameters.AddWithValue(\"@catNum\"", asmText);
            Assert.DoesNotContain("where catNumber = \" + catNumber", asmText);
        }
    }
}
