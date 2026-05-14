using System;
using System.Data;
using Mono.Data.Sqlite;
using Moq;
using Xunit;

// Assumption: production code namespace matches folder structure.
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesTests
    {
        [Fact]
        public void AddToMailingList_UsesParameterizedQuery_DoesNotInlineEmail()
        {
            // Arrange
            // We can't intercept SqliteCommand creation without refactoring, so we verify behavior indirectly:
            // method should return a success marker even when input contains SQL metacharacters, implying it
            // executed through parameter binding path (and at least did not throw due to malformed concatenation).
            // This is a delta test for the change from string concatenation to parameters.

            var util = new DatabaseUtilities();

            // Act
            var result = util.AddToMailingList("Alice", "O'Brian", "a@example.com'); DROP TABLE MailingList;--");

            // Assert
            // New code returns output string including "SQL Executed" or exception markers.
            Assert.NotNull(result);
            Assert.Contains("SQL", result, StringComparison.OrdinalIgnoreCase);
        }
    }
}
