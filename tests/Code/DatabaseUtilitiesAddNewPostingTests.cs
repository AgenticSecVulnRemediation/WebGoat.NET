using System;
using Xunit;

using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesAddNewPostingTests
    {
        [Fact]
        public void AddNewPosting_WithSqlPayload_DoesNotThrow()
        {
            // Arrange
            // Delta behavior: SQL now uses parameters passed into DoNonQuery.
            var util = new DatabaseUtilities();

            // Act
            var ex = Record.Exception(() => util.AddNewPosting("t'); DROP TABLE Postings; --", "a@b.com", "m"));

            // Assert
            Assert.Null(ex);
        }
    }
}
