using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderProductDetailsParameterizedTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedProductCodeQuery()
        {
            // Delta regression: concatenated productCode in SQL replaced with @productCode parameter.
            var src = System.IO.File.ReadAllText("WebGoat/App_Code/DB/MySqlDbProvider.cs");

            Assert.Contains("where productCode = @productCode", src);
            Assert.Contains("Parameters.AddWithValue(\"@productCode\"", src);
            Assert.DoesNotContain("where productCode = '\" + productCode + \"'", src);
        }
    }
}
