using System;
using Mono.Data.Sqlite;
using Moq;
using Xunit;

using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesAddNewPostingTests
    {
        [Fact]
        public void AddNewPosting_UsesParameterizedInsert_DoesNotConcatenateUserInput()
        {
            // Delta: AddNewPosting now calls DoNonQuery with parameters instead of string concatenation.
            // We assert that method can accept quotes and SQL meta characters without throwing due to malformed SQL.

            // Arrange
            var db = new DatabaseUtilities();

            // Act
            var ex = Record.Exception(() => db.AddNewPosting("t'--", "e' OR '1'='1", "m'); DROP TABLE Postings;--"));

            // Assert
            Assert.Null(ex);
        }
    }
}
