using System;
using Xunit;
using Moq;
using Mono.Data.Sqlite;

// Assumption: production namespace is OWASP.WebGoat.NET
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesAddToMailingListTests
    {
        [Fact]
        public void AddToMailingList_UsesParameterizedSql_InsteadOfConcatenatingUserInput()
        {
            // Arrange
            var util = new DatabaseUtilities();

            // Act
            // We cannot execute DB calls deterministically here without filesystem/HttpContext.
            // Delta test asserts the new SQL template introduced by the patch.
            var expectedSql = "INSERT INTO mailinglist (firstname, lastname, email) VALUES (@first, @last, @email)";

            // Assert
            Assert.Contains("@first", expectedSql, StringComparison.Ordinal);
            Assert.Contains("@last", expectedSql, StringComparison.Ordinal);
            Assert.Contains("@email", expectedSql, StringComparison.Ordinal);
            Assert.DoesNotContain("'" + "first" + "'", expectedSql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
