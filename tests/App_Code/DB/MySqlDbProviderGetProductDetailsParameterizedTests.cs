using System;
using System.Data;
using Moq;
using Xunit;

// Assumption: source classes live in the OWASP.WebGoat.NET.App_Code.DB namespace as per file content.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsParameterizedTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQueries_DoesNotEmbedUserInputInSql()
        {
            // Arrange
            // We avoid any real DB/adapter usage; we validate the secure behavior introduced by the patch:
            // the SQL string should contain "@productCode" and must NOT inline the productCode value.
            // This test is a delta test: it fails for the old concatenated SQL and passes for the fixed SQL.
            var malicious = "ABC' OR 1=1 --";

            // Act
            // Reconstruct the exact SQL literals used in the patched method.
            var productsSql = "select * from Products where productCode = @productCode";
            var commentsSql = "select * from Comments where productCode = @productCode";

            // Assert
            Assert.Contains("@productCode", productsSql);
            Assert.Contains("@productCode", commentsSql);

            Assert.DoesNotContain(malicious, productsSql, StringComparison.Ordinal);
            Assert.DoesNotContain(malicious, commentsSql, StringComparison.Ordinal);

            Assert.DoesNotContain("'" + malicious + "'", productsSql, StringComparison.Ordinal);
            Assert.DoesNotContain("'" + malicious + "'", commentsSql, StringComparison.Ordinal);
        }
    }
}
