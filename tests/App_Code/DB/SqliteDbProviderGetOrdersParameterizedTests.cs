using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetOrders_UsesCustomerIdParameterTests
    {
        [Fact]
        public void GetOrders_UsesCustomerIdParameterInsteadOfConcatenation()
        {
            // Delta guard for PR #430: GetOrders now uses @customerID parameter.
            var source = LoadSource();

            Assert.Contains("select * from Orders where customerNumber = @customerID", source);
            Assert.Contains("SelectCommand.Parameters.AddWithValue(\"@customerID\"", source);
            Assert.DoesNotContain("customerNumber = \" + customerID", source);
        }

        private static string LoadSource()
        {
            var path = System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "App_Code", "DB", "SqliteDbProvider.cs");
            path = System.IO.Path.GetFullPath(path);
            return System.IO.File.ReadAllText(path);
        }
    }
}
