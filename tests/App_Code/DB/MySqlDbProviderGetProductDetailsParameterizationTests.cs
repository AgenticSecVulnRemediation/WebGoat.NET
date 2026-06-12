using System.IO;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_GetProductDetailsParameterizationTests
    {
        [Fact]
        public void GetProductDetails_UsesSameParameterMarkerForProductsAndCommentsQueries()
        {
            // Delta test: GetProductDetails moved from concatenated SQL to parameterized queries.
            // Regression guard: ensure both queries use @productCode and add parameter.

            var path = Path.Combine(Directory.GetCurrentDirectory(), "WebGoat", "App_Code", "DB", "MySqlDbProvider.cs");
            Assert.True(File.Exists(path), $"Expected file at {path}");

            var text = File.ReadAllText(path);

            Assert.Contains("select * from Products where productCode = @productCode", text);
            Assert.Contains("select * from Comments where productCode = @productCode", text);
            Assert.Contains("da.SelectCommand.Parameters.AddWithValue(\"@productCode\"", text);

            // Old vulnerable patterns should not exist.
            Assert.DoesNotContain("productCode = '\" + productCode", text);
        }
    }
}
