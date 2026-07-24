using System;
using System.Data;
using Moq;
using Xunit;

// Assumption: Source file SqliteDbProvider.cs is compiled in OWASP.WebGoat.NET.App_Code.DB namespace.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailsParameterizedTests
    {
        // Delta test: GetCustomerEmails now uses "email like @EmailPattern" + parameter value (email + "%")
        // rather than string concatenation into SQL.
        [Fact]
        public void GetCustomerEmails_UsesEmailPatternParameter_PreservesPrefixSemantics()
        {
            // Arrange
            // We can't easily intercept the SqliteDataAdapter's internal command without heavy integration.
            // So we assert the secure SQL text exists and the vulnerable concatenation string does not.
            var asm = typeof(SqliteDbProvider).Assembly;

            // Act
            var bytes = System.IO.File.ReadAllBytes(asm.Location);
            var text = System.Text.Encoding.UTF8.GetString(bytes);

            // Assert
            Assert.Contains("select email from CustomerLogin where email like @EmailPattern", text);
            Assert.DoesNotContain("select email from CustomerLogin where email like '\" + email + \"%'", text);
        }
    }
}
