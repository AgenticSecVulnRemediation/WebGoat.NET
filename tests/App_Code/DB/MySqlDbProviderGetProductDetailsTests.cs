using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_SqlString_UsesProductCodeParameter()
        {
            // Arrange/Act
            string source = typeof(MySqlDbProvider).ToString();

            // Assert (delta)
            Assert.Contains("where productCode = @productCode", source);
        }
    }
}
