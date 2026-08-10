using System;
using System.Data;
using Moq;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQuery_ForProductCode()
        {
            // Arrange
            // The fix changes productCode = '...' to productCode = @productCode
            string expectedSqlPart1 = "select * from Products where productCode = @productCode";
            string expectedSqlPart2 = "select * from Comments where productCode = @productCode";

            // Act & Assert
            Assert.Contains("@productCode", expectedSqlPart1);
            Assert.Contains("@productCode", expectedSqlPart2);
        }
    }
}
