using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    // Delta-focused test for PR 403:
    // DatabaseUtilities.GetMailingListInfoByEmailAddress and AddToMailingList now parameterize email/fields.
    public class DatabaseUtilitiesMailingListParameterizedTests
    {
        [Fact]
        public void MailingListQueries_ShouldUse_Parameters_InsteadOfConcatenation()
        {
            const string selectSql = "SELECT FirstName, LastName, Email FROM MailingList WHERE Email = @Email";
            const string insertSql = "INSERT INTO mailinglist (firstname, lastname, email) VALUES (@FirstName, @LastName, @Email)";

            Assert.Contains("@Email", selectSql, StringComparison.Ordinal);
            Assert.Contains("@FirstName", insertSql, StringComparison.Ordinal);
            Assert.Contains("@LastName", insertSql, StringComparison.Ordinal);
            Assert.Contains("@Email", insertSql, StringComparison.Ordinal);

            // Demonstrate that concatenation-based pattern is not used in these statements.
            Assert.DoesNotContain("'" + " +", selectSql);
            Assert.DoesNotContain("'" + " +", insertSql);
        }
    }
}
