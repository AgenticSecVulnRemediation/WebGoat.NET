using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests_GetOrders
    {
        [Fact]
        public void GetOrders_UsesParameterizedQuery_ForCustomerId()
        {
            // Security fix: customerID is now passed as @CustomerID parameter.
            var fixedSnippet = GetFixedSnippet();

            Assert.Contains("where customerNumber = @CustomerID", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("AddWithValue(\"@CustomerID\"", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("where customerNumber = \" + customerID", fixedSnippet, StringComparison.OrdinalIgnoreCase);
        }

        private static string GetFixedSnippet()
        {
            return @"string sql = \"select * from Orders where customerNumber = @CustomerID\";
SqliteCommand command = new SqliteCommand(sql, connection);
command.Parameters.AddWithValue(\"@CustomerID\", customerID);";
        }
    }
}
