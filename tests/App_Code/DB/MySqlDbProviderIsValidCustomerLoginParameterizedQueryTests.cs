using System;
using System.Data;
using Moq;
using Xunit;

// Assumption: source namespace is OWASP.WebGoat.NET.App_Code.DB based on file_path.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderIsValidCustomerLoginParameterizedQueryTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameterizedQuery_ForEmailAndPassword()
        {
            // Arrange
            // We validate the SQL text change by scanning method body (string literal) in assembly.
            var asmText = System.Text.Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(typeof(MySqlDbProvider).Assembly.Location));

            // Assert
            Assert.Contains("select * from CustomerLogin where email = @email and password = @password;", asmText);
            Assert.DoesNotContain("select * from CustomerLogin where email = '\"", asmText);
        }
    }
}
