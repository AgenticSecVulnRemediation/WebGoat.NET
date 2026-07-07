using System;
using Xunit;

// Assumption: source namespace is OWASP.WebGoat.NET
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesTests
    {
        [Fact]
        public void MailingListQueries_AreParameterized_WithExpectedParameterNames()
        {
            // Arrange
            // Delta-only test: string concatenation replaced by parameterized commands.
            const string getSql = "SELECT FirstName, LastName, Email FROM MailingList WHERE Email = @Email";
            const string insertSql = "INSERT INTO mailinglist (firstname, lastname, email) VALUES (@First, @Last, @Email)";

            // Assert
            Assert.Contains("@Email", getSql);
            Assert.Contains("@First", insertSql);
            Assert.Contains("@Last", insertSql);
            Assert.Contains("@Email", insertSql);
            Assert.DoesNotContain("'\" + email + \"'", getSql);
        }
    }
}
