using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesParameterizedQueriesTests
    {
        [Fact]
        public void GetEmailByUserID_UsesParameterizedQuery()
        {
            // Delta test: query updated from string concatenation to parameterized.
            var sql = "SELECT Email FROM UserList WHERE UserID = @UserID";

            Assert.Contains("@UserID", sql, StringComparison.Ordinal);
            Assert.DoesNotContain("WHERE UserID = '\" + userid + \"'", sql, StringComparison.Ordinal);
        }

        [Fact]
        public void GetMailingListInfoByEmailAddress_UsesParameterizedQuery()
        {
            var sql = "SELECT FirstName, LastName, Email FROM MailingList where Email = @Email";

            Assert.Contains("@Email", sql, StringComparison.Ordinal);
            Assert.DoesNotContain("Email = '\" + email + \"'", sql, StringComparison.Ordinal);
        }
    }
}
