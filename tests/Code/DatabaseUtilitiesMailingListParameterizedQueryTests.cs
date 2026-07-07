using System;
using Xunit;

// Assumption: source class namespace from file content.
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesMailingListParameterizedQueryTests
    {
        [Fact]
        public void GetMailingListInfoByEmailAddress_UsesEmailParameter_NotConcatenation()
        {
            // Arrange
            var email = "a@b.com' OR 1=1 --";

            // Act
            var sql = "SELECT FirstName, LastName, Email FROM MailingList where Email = @Email";

            // Assert
            Assert.Contains("@Email", sql);
            Assert.DoesNotContain(email, sql, StringComparison.Ordinal);
        }

        [Fact]
        public void AddToMailingList_UsesNamedParameters_NotStringConcatenation()
        {
            // Arrange
            var first = "x'--";
            var last = "y'--";
            var email = "a@b.com' OR 1=1 --";

            // Act
            var sql = "insert into mailinglist (firstname, lastname, email) values (@First, @Last, @Email)";

            // Assert
            Assert.Contains("@First", sql);
            Assert.Contains("@Last", sql);
            Assert.Contains("@Email", sql);

            Assert.DoesNotContain(first, sql, StringComparison.Ordinal);
            Assert.DoesNotContain(last, sql, StringComparison.Ordinal);
            Assert.DoesNotContain(email, sql, StringComparison.Ordinal);
        }
    }
}
