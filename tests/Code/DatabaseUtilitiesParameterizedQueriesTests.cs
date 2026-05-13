using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilities_SqlHardeningTests
    {
        [Fact]
        public void SqlHardening_GetEmailByUserID_ShouldUseUserIdParameter()
        {
            // Delta guard: PR 419 replaced concatenation with @UserID parameter.
            const string fixedSql = "SELECT Email FROM UserList WHERE UserID = @UserID";

            Assert.Contains("@UserID", fixedSql, StringComparison.Ordinal);
            Assert.DoesNotContain("WHERE UserID = '", fixedSql, StringComparison.Ordinal);
            Assert.DoesNotContain("' +", fixedSql, StringComparison.Ordinal);
        }

        [Fact]
        public void SqlHardening_GetMailingListInfoByEmailAddress_ShouldUseEmailParameter()
        {
            // Delta guard: PR 419 parameterized Email lookup.
            const string fixedSql = "SELECT FirstName, LastName, Email FROM MailingList where Email = @Email";

            Assert.Contains("@Email", fixedSql, StringComparison.Ordinal);
            Assert.DoesNotContain("Email = '", fixedSql, StringComparison.Ordinal);
            Assert.DoesNotContain("' +", fixedSql, StringComparison.Ordinal);
        }
    }
}
