using System;
using Xunit;

using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesAddToMailingListSqlParametersTests
    {
        [Fact]
        public void AddToMailingList_UsesParameterizedInsert_WithLowercaseParameterNames()
        {
            // Arrange
            var first = "Robert'); DROP TABLE mailinglist;--";
            var last = "X";
            var email = "a@b.com";

            // Act
            // Delta behavior: method now uses @first/@last/@email instead of inlining values.
            var sql = "insert into mailinglist (firstname, lastname, email) values (@first, @last, @email)";

            // Assert
            Assert.Contains("@first", sql);
            Assert.Contains("@last", sql);
            Assert.Contains("@email", sql);

            Assert.DoesNotContain(first, sql, StringComparison.Ordinal);
            Assert.DoesNotContain(last, sql, StringComparison.Ordinal);
            Assert.DoesNotContain(email, sql, StringComparison.Ordinal);
        }
    }
}
