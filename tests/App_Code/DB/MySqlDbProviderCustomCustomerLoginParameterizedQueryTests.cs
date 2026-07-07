using System;
using System.IO;
using Xunit;

// Assumption: source namespace is OWASP.WebGoat.NET.App_Code.DB based on file_path.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderCustomCustomerLoginParameterizedQueryTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterizedQuery_ForEmailLookup()
        {
            // Arrange
            // Patch changes email lookup query from string concatenation to parameterized "@email".
            var asmText = System.Text.Encoding.UTF8.GetString(File.ReadAllBytes(typeof(MySqlDbProvider).Assembly.Location));

            // Assert
            Assert.Contains("select * from CustomerLogin where email = @email;", asmText);
            Assert.DoesNotContain("select * from CustomerLogin where email = '\"", asmText);
        }
    }
}
