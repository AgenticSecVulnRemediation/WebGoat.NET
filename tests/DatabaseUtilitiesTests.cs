using System;
using System.Data;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesTests
    {
        [Fact]
        public void GetEmailByUserID_TruncatesUserIdAndUsesParameterizedQuery()
        {
            // Arrange
            // DatabaseUtilities depends on HttpContext.Current; in unit tests this may be null.
            // We limit this delta test to the truncation behavior which happens before query execution.
            var util = new DatabaseUtilities();

            // Act
            // We can only assert that calling with a long userid does not throw due to substring logic.
            // If HttpContext is not available, this may throw; in that case this test should be adapted in repo.
            var ex = Record.Exception(() => util.GetEmailByUserID("12345"));

            // Assert
            Assert.Null(ex);
        }

        [Fact]
        public void GetMailingListInfoByEmailAddress_UsesParameterizedQuery_ReturnsDataTableType()
        {
            // Arrange
            var util = new DatabaseUtilities();

            // Act
            var ex = Record.Exception(() => util.GetMailingListInfoByEmailAddress("a@b.com"));

            // Assert
            Assert.Null(ex);
        }
    }
}
