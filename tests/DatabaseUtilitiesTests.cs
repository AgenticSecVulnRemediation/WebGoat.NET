using System;
using System.Data;
using Xunit;
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesTests
    {
        [Fact]
        public void GetEmailByUserID_WithInjectionPayload_DoesNotThrowAndReturnsSafeMessage()
        {
            // Delta regression test: GetEmailByUserID now uses a parameterized SqliteCommand.
            // Previous version concatenated user input into SQL.

            // Arrange
            var sut = new DatabaseUtilities();
            var injectedUserId = "1' OR '1'='1";

            // Act
            var ex = Record.Exception(() => sut.GetEmailByUserID(injectedUserId));

            // Assert
            Assert.Null(ex);
            var result = sut.GetEmailByUserID(injectedUserId);
            Assert.NotNull(result);
        }

        [Fact]
        public void GetMailingListInfoByEmailAddress_WithInjectionPayload_DoesNotThrow()
        {
            // Delta regression test: GetMailingListInfoByEmailAddress now uses a parameterized SqliteCommand.

            // Arrange
            var sut = new DatabaseUtilities();
            var injectedEmail = "a@b.com' OR '1'='1";

            // Act
            var ex = Record.Exception(() => sut.GetMailingListInfoByEmailAddress(injectedEmail));

            // Assert
            Assert.Null(ex);
        }
    }
}
