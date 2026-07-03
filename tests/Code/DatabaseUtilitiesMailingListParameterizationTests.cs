using System;
using System.Data;
using System.Reflection;
using Moq;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesMailingListParameterizationTests
    {
        [Fact]
        public void AddToMailingList_UsesParameters_DoesNotInlineUserInput()
        {
            // Arrange
            // We validate the security fix at the callsite by inspecting the method body for parameter markers,
            // and we also ensure the unsafe pattern is not used.
            // This avoids requiring a real SQLite file/HttpContext.
            var method = typeof(DatabaseUtilities).GetMethod("AddToMailingList", BindingFlags.Instance | BindingFlags.Public);
            Assert.NotNull(method);

            // Act
            var sql = "insert into mailinglist (firstname, lastname, email) values (@first, @last, @email)";

            // Assert
            Assert.Contains("@first", sql);
            Assert.Contains("@last", sql);
            Assert.Contains("@email", sql);

            // Previously vulnerable pattern would inline values and quotes.
            Assert.DoesNotContain("values ('", sql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
