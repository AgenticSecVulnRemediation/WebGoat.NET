using System;
using Mono.Data.Sqlite;
using Xunit;

// Assumption: DatabaseUtilities is in namespace OWASP.WebGoat.NET
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesTests
    {
        [Fact]
        public void GetEmailByUserID_WithSqlPayload_DoesNotThrow()
        {
            // Arrange
            // Delta behavior: query now uses @UserID parameter and helper supports parameters.
            var util = new DatabaseUtilities();

            // Act
            var ex = Record.Exception(() => util.GetEmailByUserID("' OR 1=1 --"));

            // Assert
            Assert.Null(ex);
        }
    }
}
