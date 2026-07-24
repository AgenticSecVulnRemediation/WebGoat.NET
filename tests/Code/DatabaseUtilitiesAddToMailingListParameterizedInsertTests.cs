using Xunit;
using System;

using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesAddToMailingListParameterizedInsertTests
    {
        [Fact]
        public void AddToMailingList_UsesParameters_NotConcatenatedValues()
        {
            // Arrange
            var attackerFirst = "x', (SELECT sqlite_version()), 'y";

            // Act
            var sql = "insert into mailinglist (firstname, lastname, email) values (@First, @Last, @Email)";

            // Assert
            Assert.Contains("@First", sql, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("@Last", sql, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("@Email", sql, StringComparison.OrdinalIgnoreCase);

            // Key security delta: do not inline user-controlled values.
            Assert.DoesNotContain(attackerFirst, sql, StringComparison.Ordinal);
            Assert.DoesNotContain("values ('", sql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
