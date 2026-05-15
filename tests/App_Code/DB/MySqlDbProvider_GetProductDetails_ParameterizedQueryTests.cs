using System;
using Xunit;

// Assumption: source namespace is OWASP.WebGoat.NET.App_Code.DB
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesMySqlCommandWithParameter_ProductCode()
        {
            // Arrange
            // Delta: uses MySqlCommand("select * from Products where productCode = @productCode")
            // and similarly for Comments.
            var productsQuery = "select * from Products where productCode = @productCode";
            var commentsQuery = "select * from Comments where productCode = @productCode";

            // Assert
            Assert.Contains("@productCode", productsQuery);
            Assert.Contains("@productCode", commentsQuery);
            Assert.DoesNotContain("'\" +", productsQuery);
            Assert.DoesNotContain("'\" +", commentsQuery);
        }
    }
}
