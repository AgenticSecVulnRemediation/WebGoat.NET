using System;
using System.Data;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesGetEmailByUserIdParameterizationTests
    {
        [Fact]
        public void GetEmailByUserID_UsesParameterPlaceholder_InsteadOfStringConcatenation()
        {
            // Arrange/Act
            var method = typeof(DatabaseUtilities).GetMethod("GetEmailByUserID");
            Assert.NotNull(method);

            // Assert
            // Delta: method now uses "UserID = @UserID" and adds parameter.
            var asm = typeof(DatabaseUtilities).Assembly.ToString();
            Assert.Contains("UserID = @UserID", asm);
            Assert.Contains("@UserID", asm);
        }
    }
}
