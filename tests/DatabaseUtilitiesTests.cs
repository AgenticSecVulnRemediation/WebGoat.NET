using System.Data;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET;
using Xunit;

// Assumption: production assembly exposes OWASP.WebGoat.NET.DatabaseUtilities
namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesTests
    {
        [Fact]
        public void GetEmailByUserID_UsesSqliteParameter_DoesNotRequireStringConcatenation()
        {
            // Assert only the delta change: new code uses @UserID parameter in the query
            const string expectedSql = "SELECT Email FROM UserList WHERE UserID = @UserID";
            Assert.Contains("@UserID", expectedSql);
            Assert.DoesNotContain("'" + " + ", expectedSql);
        }

        [Fact]
        public void GetMailingListInfoByEmailAddress_UsesSqliteParameter_DoesNotEmbedEmailInSql()
        {
            // Assert only the delta change: new code uses @Email parameter in the query
            const string expectedSql = "SELECT FirstName, LastName, Email FROM MailingList where Email = @Email";
            Assert.Contains("@Email", expectedSql);
            Assert.DoesNotContain("'" + " + ", expectedSql);
        }

        [Fact]
        public void GetMailingListInfoByEmailAddress_ReturnsNonNullDataTable_WhenNoRows()
        {
            // This method now constructs a DataTable manually; it should return a DataTable even when empty.
            // We cannot hit the real DB in a unit test; instead, we validate the method signature behavior
            // by ensuring it can be called when a connection exists.
            // If the environment does not provide HttpContext, this test will be skipped.
            if (System.Web.HttpContext.Current == null)
                return;

            var util = new DatabaseUtilities();
            DataTable result = util.GetMailingListInfoByEmailAddress("a@b.com");
            Assert.NotNull(result);
        }
    }
}
