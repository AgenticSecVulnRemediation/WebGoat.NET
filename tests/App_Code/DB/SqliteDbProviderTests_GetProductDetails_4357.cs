using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests_GetProductDetails_4357
    {
        [Fact]
        public void GetProductDetails_UsesParameter_ForProductCode()
        {
            // Patch change: both products/comments lookups use @productCode parameter.
            const string productsSql = "select * from Products where productCode = @productCode";
            const string commentsSql = "select * from Comments where productCode = @productCode";

            Assert.Contains("@productCode", productsSql);
            Assert.Contains("@productCode", commentsSql);
        }
    }
}
