using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedProductCode()
        {
            // Arrange
            var asmText = System.IO.File.ReadAllText(typeof(MySqlDbProvider).Assembly.Location);

            // Assert
            Assert.Contains("select * from Products where productCode = @productCode", asmText);
            Assert.Contains("cmd.Parameters.AddWithValue(\"@productCode\"", asmText);
            Assert.Contains("select * from Comments where productCode = @productCode", asmText);
        }
    }
}
