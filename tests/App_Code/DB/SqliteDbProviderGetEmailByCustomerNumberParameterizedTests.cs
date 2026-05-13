using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetEmailByCustomerNumber_UsesParameterTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesNumParameter()
        {
            // Delta guard for PR #435: GetEmailByCustomerNumber now uses @num parameter.
            var source = LoadSource();

            Assert.Contains("where customerNumber = @num", source);
            Assert.Contains("Parameters.AddWithValue(\"@num\"", source);
            Assert.DoesNotContain("customerNumber = \" + num", source);
        }

        private static string LoadSource()
        {
            var path = System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "App_Code", "DB", "SqliteDbProvider.cs");
            path = System.IO.Path.GetFullPath(path);
            return System.IO.File.ReadAllText(path);
        }
    }
}
