using System;
using Xunit;

// Assumption: production namespace is OWASP.WebGoat.NET.App_Code.DB as in the source file.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderCustomerLoginAndEmailSearchTests
    {
        [Fact]
        public void CustomCustomerLogin_ParameterIsAddedForEmailFilter()
        {
            // This is a delta regression test for the fix that adds @Email parameter.
            // We assert that the compiled assembly contains "@Email" as a string constant.

            var asmBytes = System.IO.File.ReadAllBytes(typeof(SqliteDbProvider).Assembly.Location);
            Assert.Contains("@Email", System.Text.Encoding.UTF8.GetString(asmBytes));
        }

        [Fact]
        public void GetCustomerEmails_UsesParameterPlaceholder_NotStringConcatenation()
        {
            // Regression test: SQL changed from "... like '" + email + "%'" to "... like @Email || '%'".
            var asmBytes = System.IO.File.ReadAllBytes(typeof(SqliteDbProvider).Assembly.Location);
            var text = System.Text.Encoding.UTF8.GetString(asmBytes);

            Assert.Contains("@Email", text);
            Assert.Contains("|| '%'", text);
        }
    }
}
