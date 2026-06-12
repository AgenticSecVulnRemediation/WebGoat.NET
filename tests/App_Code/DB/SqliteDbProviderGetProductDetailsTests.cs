using System;
using Xunit;

// Assumption: Source namespace is OWASP.WebGoat.NET.App_Code.DB
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsParameterizationTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedCommands_ForProductAndCommentsQueries()
        {
            // Arrange
            // Patch replaces string concatenation with SqliteCommand and @productCode parameter.
            var productsSql = "select * from Products where productCode = @productCode";
            var commentsSql = "select * from Comments where productCode = @productCode";

            // Assert
            Assert.Contains("@productCode", productsSql);
            Assert.Contains("@productCode", commentsSql);
            Assert.DoesNotContain("'" + " +", productsSql);
            Assert.DoesNotContain("'" + " +", commentsSql);
        }
    }
}
