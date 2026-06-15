using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductsAndCategoriesTests
    {
        [Fact]
        public void GetProductsAndCategories_WithCatNumber_UsesParameterizedWhereClause()
        {
            // Regression test: catNumber filter is parameterized.
            var path = System.IO.Path.Combine("WebGoat", "App_Code", "DB", "MySqlDbProvider.cs");
            var content = System.IO.File.ReadAllText(path);

            Assert.Contains("where catNumber = @catNumber", content);
            Assert.Contains("AddWithValue(\"@catNumber\"", content);
            Assert.DoesNotContain("where catNumber = \" + catNumber", content);
        }
    }
}
