using System;
using System.Data;
using Moq;
using Xunit;

// Assumption: Source is in OWASP.WebGoat.NET.App_Code.DB namespace.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderIsValidCustomerLoginParameterizedTests
    {
        // Delta test: IsValidCustomerLogin now uses parameters (@Email, @Password) instead of string concatenation.
        [Fact]
        public void IsValidCustomerLogin_SourceContainsParameterizedWhereClause()
        {
            var asm = typeof(MySqlDbProvider).Assembly;
            var bytes = System.IO.File.ReadAllBytes(asm.Location);
            var text = System.Text.Encoding.UTF8.GetString(bytes);

            Assert.Contains("select * from CustomerLogin where email = @Email and password = @Password;", text);
            Assert.DoesNotContain("select * from CustomerLogin where email = '\" + email", text);
        }
    }
}
