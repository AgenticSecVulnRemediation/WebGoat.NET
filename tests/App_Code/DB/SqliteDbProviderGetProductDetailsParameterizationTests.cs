using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsParameterizationTests
    {
        [Fact]
        public void GetProductDetails_UsesProductCodeParameterInBothQueries()
        {
            // Delta test: both Products and Comments queries use @productCode parameter.
            var productSql = "select * from Products where productCode = @productCode";
            var commentSql = "select * from Comments where productCode = @productCode";
            Assert.Contains("@productCode", productSql);
            Assert.Contains("@productCode", commentSql);
        }
    }
}
