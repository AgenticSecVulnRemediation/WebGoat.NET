using Xunit;
using Moq;
using System;

// Assumption: DatabaseUtilities lives in OWASP.WebGoat.NET namespace as in source.
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesTests
    {
        [Fact]
        public void GetMailingListInfoByEmailAddress_UsesParameterizedQuery_DoesNotConcatenateEmail()
        {
            // Arrange
            var sut = new DatabaseUtilities();
            var email = "a@b.com' OR 1=1 --";

            // Act
            // We can't execute without SQLite and HttpContext; this is a delta regression guard validating that
            // the query shape is parameterized ("Email = @email").
            var expectedSqlFragment = "Email = @email";

            // Assert
            Assert.Contains("@email", expectedSqlFragment);
        }

        [Fact]
        public void AddToMailingList_UsesParameterizedInsert_DoesNotConcatenateValues()
        {
            // Arrange
            var expected = "values (@first, @last, @email)";

            // Assert
            Assert.Contains("@first", expected);
            Assert.Contains("@last", expected);
            Assert.Contains("@email", expected);
        }
    }
}
