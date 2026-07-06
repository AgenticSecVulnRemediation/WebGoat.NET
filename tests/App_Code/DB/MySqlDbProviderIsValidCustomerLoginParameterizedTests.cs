using System;
using System.Data;
using Moq;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderIsValidCustomerLoginParameterizedTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameterizedQueryTemplate_InSource()
        {
            // Delta test: IsValidCustomerLogin moved from string concatenation to parameterized SQL.
            // Because MySql types are external and DB is not available here, assert via source-level check.

            var path = System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "App_Code", "DB", "MySqlDbProvider.cs");
            if (!System.IO.File.Exists(path))
            {
                throw new InvalidOperationException($"Expected source file not found at {path}");
            }

            var text = System.IO.File.ReadAllText(path);

            Assert.Contains("select * from CustomerLogin where email = @email and password = @password", text);
            Assert.Contains("cmd.Parameters.AddWithValue(\"@email\"", text);
            Assert.Contains("cmd.Parameters.AddWithValue(\"@password\"", text);

            // Negative assertion: old concatenation pattern should not exist in the method anymore.
            Assert.DoesNotContain("where email = '\" + email", text);
        }
    }
}
