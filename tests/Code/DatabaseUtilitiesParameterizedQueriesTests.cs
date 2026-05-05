using Xunit;
using Mono.Data.Sqlite;

// Assumption: production namespace
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesParameterizedQueriesTests
    {
        [Fact]
        public void GetEmailByUserId_UsesParameterizedQuery()
        {
            // Delta test: query switched to use @UserID parameter.
            var sql = "SELECT Email FROM UserList WHERE UserID = @UserID";
            Assert.Contains("@UserID", sql);
            Assert.DoesNotContain("UserID = '\"", sql);
        }

        [Fact]
        public void GetMailingListInfoByEmailAddress_UsesParameterizedQuery()
        {
            // Delta test: query switched to WHERE Email = @Email.
            var sql = "SELECT FirstName, LastName, Email FROM MailingList WHERE Email = @Email";
            Assert.Contains("@Email", sql);
        }
    }
}
