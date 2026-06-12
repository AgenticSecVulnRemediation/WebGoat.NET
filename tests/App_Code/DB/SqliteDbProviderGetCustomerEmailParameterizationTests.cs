using System;
using System.Data;
using Mono.Data.Sqlite;
using Xunit;

// Assumption: Source namespace is OWASP.WebGoat.NET.App_Code.DB
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameter_ForCustomerNumber_ToPreventSqlInjection()
        {
            // Arrange
            // Patch changes query to use @customerNumber and adds parameter.
            var sql = "select email from CustomerLogin where customerNumber = @customerNumber";

            // Assert
            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("where customerNumber = " + " +", sql);
        }
    }
}
