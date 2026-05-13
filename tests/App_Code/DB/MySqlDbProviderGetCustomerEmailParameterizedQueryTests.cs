using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_GetCustomerEmail_ParameterizedQueryTests
    {
        [Fact]
        public void GetCustomerEmail_UsesCustomerNumberParameter_InQuery()
        {
            // Delta guard for PR #385: GetCustomerEmail now parameterizes @customerNumber.
            var source = LoadSource();

            Assert.Contains("select email from CustomerLogin where customerNumber = @customerNumber", source);
            Assert.Contains("Parameters.AddWithValue(\"@customerNumber\"", source);
            // Ensure regression doesn't reintroduce concatenation in that query.
            Assert.DoesNotContain("where customerNumber = \" + customerNumber", source);
        }

        private static string LoadSource()
        {
            var path = System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "App_Code", "DB", "MySqlDbProvider.cs");
            path = System.IO.Path.GetFullPath(path);
            return System.IO.File.ReadAllText(path);
        }
    }
}
