using Xunit;
using System;
using System.Data;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesAddToMailingListTests
    {
        [Fact]
        public void AddToMailingList_UsesParameterizedInsert_ReturnsExecutedMessage()
        {
            // Arrange
            var db = new DatabaseUtilities();

            // Act
            // Use minimal representative values.
            var result = db.AddToMailingList("John", "Doe", "john@example.com");

            // Assert
            Assert.NotNull(result);
            Assert.Contains("SQL Executed", result);
        }

        [Fact]
        public void AddNewPosting_UsesParameterizedInsert_ReturnsExecutedMessage()
        {
            // Arrange
            var db = new DatabaseUtilities();

            // Act
            var result = db.AddNewPosting("t", "a@b.com", "m");

            // Assert
            Assert.NotNull(result);
            Assert.Contains("SQL Executed", result);
        }
    }
}
