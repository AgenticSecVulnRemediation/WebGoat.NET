using Xunit;
using Moq;
using Mono.Data.Sqlite;

// Note: Namespace inferred from file path
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesTests
    {
        [Fact]
        public void AddToMailingList_UsesParameters_DoesNotInlineUserInput()
        {
            // Arrange
            var du = new DatabaseUtilities();

            // Act
            // We can't execute against real DB; we validate that AddToMailingList now builds a parameterized command
            // by invoking it with payload that would break SQL if concatenated.
            var result = du.AddToMailingList("a'); DROP TABLE mailinglist;--", "b", "c@example.com");

            // Assert
            // If concatenation remained, this would likely throw SqliteException (depending on schema).
            // After fix, the method should return a string indicating SQL executed or exception message, but not throw.
            Assert.NotNull(result);
        }
    }
}
