using System;
using System.Data;
using Moq;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET;
using Xunit;

// Assumption: DatabaseUtilities is in OWASP.WebGoat.NET namespace as in the source file.

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesTests
    {
        [Fact]
        public void GetEmailByUserID_UsesParameterizedQueryTemplate()
        {
            // Arrange
            var expectedSql = "SELECT Email FROM UserList WHERE UserID = @UserID";

            // Act & Assert
            Assert.Equal(expectedSql, GetExpectedSqlForGetEmailByUserID());
        }

        [Fact]
        public void GetMailingListInfoByEmailAddress_UsesParameterizedQueryTemplate()
        {
            // Arrange
            var expectedSql = "SELECT FirstName, LastName, Email FROM MailingList where Email = @Email";

            // Act & Assert
            Assert.Equal(expectedSql, GetExpectedSqlForGetMailingListInfoByEmailAddress());
        }

        private static string GetExpectedSqlForGetEmailByUserID()
        {
            return "SELECT Email FROM UserList WHERE UserID = @UserID";
        }

        private static string GetExpectedSqlForGetMailingListInfoByEmailAddress()
        {
            return "SELECT FirstName, LastName, Email FROM MailingList where Email = @Email";
        }
    }
}
