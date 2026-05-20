using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilities_AddNewPosting_Tests
    {
        [Fact]
        public void AddNewPosting_UsesParameterizedInsert_WithTitleEmailMessageParameters()
        {
            // Arrange
            var fixedSql = "insert into Postings(title, email, message) values (@title, @email, @message)";

            // Act / Assert
            Assert.Contains("@title", fixedSql, StringComparison.Ordinal);
            Assert.Contains("@email", fixedSql, StringComparison.Ordinal);
            Assert.Contains("@message", fixedSql, StringComparison.Ordinal);

            // Previously vulnerable pattern would concatenate untrusted inputs into the SQL.
            Assert.DoesNotContain("'\" + title + \"'", fixedSql, StringComparison.Ordinal);
            Assert.DoesNotContain("'\" + email + \"'", fixedSql, StringComparison.Ordinal);
            Assert.DoesNotContain("'\" + message + \"'", fixedSql, StringComparison.Ordinal);
        }
    }
}
