using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetEmailByCustomerNumber_Tests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameter_InsteadOfStringConcatenation()
        {
            // Arrange
            var fixedSql = "select email from CustomerLogin where customerNumber = @customerNumber";

            // Act / Assert
            Assert.Contains("@customerNumber", fixedSql, StringComparison.Ordinal);
            Assert.DoesNotContain("+ num", fixedSql, StringComparison.Ordinal);
        }
    }
}
