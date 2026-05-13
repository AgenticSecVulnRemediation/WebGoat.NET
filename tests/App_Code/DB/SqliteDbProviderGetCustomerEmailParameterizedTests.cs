using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetCustomerEmail_UsesCustomerIdParameterTests
    {
        [Fact]
        public void GetCustomerEmail_UsesCustomerNumberParameter()
        {
            // Delta guard for PR #432: GetCustomerEmail now uses @customerNumber parameter.
            var source = LoadSource();

            Assert.Contains("customerNumber=@customerNumber", source);
            Assert.Contains("Parameters.AddWithValue(\"@customerNumber\"", source);
            Assert.DoesNotContain("customerNumber = \" + customerNumber", source);
        }

        private static string LoadSource()
        {
            var path = System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "App_Code", "DB", "SqliteDbProvider.cs");
            path = System.IO.Path.GetFullPath(path);
            return System.IO.File.ReadAllText(path);
        }
    }
}
