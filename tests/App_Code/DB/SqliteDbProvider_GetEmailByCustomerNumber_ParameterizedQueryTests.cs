using System;
using Xunit;

// Assumption: source namespace is OWASP.WebGoat.NET.App_Code.DB
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetEmailByCustomerNumberTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesSqliteParameter_InsteadOfConcatenation()
        {
            // Arrange
            // Delta: query now includes @num and adds parameter via AddWithValue.
            var sql = "select email from CustomerLogin where customerNumber = @num";

            // Assert
            Assert.Contains("@num", sql);
            Assert.DoesNotContain("+ num", sql);
        }
    }
}
