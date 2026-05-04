using System;
using OWASP.WebGoat.NET;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesInsertTests
    {
        [Fact]
        public void AddNewPosting_UsesParameterizedInsertPlaceholders()
        {
            const string expectedSql = "insert into Postings(title, email, message) values (@title, @email, @message)";
            Assert.Contains("@title", expectedSql);
            Assert.Contains("@email", expectedSql);
            Assert.Contains("@message", expectedSql);
            Assert.DoesNotContain("values ('", expectedSql, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void AddToMailingList_UsesParameterizedInsertPlaceholders()
        {
            const string expectedSql = "insert into mailinglist (firstname, lastname, email) values (@first, @last, @email)";
            Assert.Contains("@first", expectedSql);
            Assert.Contains("@last", expectedSql);
            Assert.Contains("@email", expectedSql);
            Assert.DoesNotContain("values ('", expectedSql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
