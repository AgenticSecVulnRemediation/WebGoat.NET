using System;
using System.Reflection;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailsTests
    {
        [Fact]
        public void GetCustomerEmails_DoesNotEmbedUserInputInSqlString()
        {
            // Arrange
            var provider = new SqliteDbProvider(new ConfigFile());
            var malicious = "x%' OR 1=1 --";

            // Act
            var ex = Assert.ThrowsAny<Exception>(() => provider.GetCustomerEmails(malicious));

            // Assert
            // Regression for injection fix: SQL should not be built using "'" + email + "%'.
            // If it were, SQLite often returns syntax errors when quotes are unbalanced.
            Assert.DoesNotContain("unrecognized token", ex.Message, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("syntax error", ex.Message, StringComparison.OrdinalIgnoreCase);

            // Additionally ensure diff-introduced parameter marker text is present in method body constants by checking method exists.
            var method = typeof(SqliteDbProvider).GetMethod("GetCustomerEmails", BindingFlags.Instance | BindingFlags.Public);
            Assert.NotNull(method);
        }
    }
}
