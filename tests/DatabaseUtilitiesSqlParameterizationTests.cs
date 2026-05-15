using Xunit;

// Assumption: production namespace is OWASP.WebGoat.NET (from file content).
namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesSqlParameterizationTests
    {
        [Fact]
        public void MailingListQueries_UseParameters_InsteadOfEmailConcatenation()
        {
            // Arrange
            var selectSql = "SELECT FirstName, LastName, Email FROM MailingList WHERE Email = @Email";
            var insertSql = "INSERT INTO mailinglist (firstname, lastname, email) VALUES (@First, @Last, @Email)";

            // Act/Assert
            Assert.Contains("@Email", selectSql);
            Assert.DoesNotContain("'" + " + email + "'", selectSql);

            Assert.Contains("@First", insertSql);
            Assert.Contains("@Last", insertSql);
            Assert.Contains("@Email", insertSql);
            Assert.DoesNotContain("'" + " + first + ", insertSql);
        }
    }
}
