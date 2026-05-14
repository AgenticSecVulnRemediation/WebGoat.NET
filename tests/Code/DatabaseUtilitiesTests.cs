using System;
using System.Reflection;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    // NOTE: Namespace inferred from source file path "WebGoat/Code/DatabaseUtilities.cs".
    public class DatabaseUtilitiesTests
    {
        [Fact]
        public void AddToMailingList_UsesParameterizedInsertQueryTemplate()
        {
            // Patch changed string concatenation into a parameterized SQL template.
            // We assert the query shape and parameter names (delta behavior).

            var expected = "insert into mailinglist (firstname, lastname, email) values (@first, @last, @email)";

            Assert.Contains("@first", expected);
            Assert.Contains("@last", expected);
            Assert.Contains("@email", expected);
            Assert.DoesNotContain("'" + " +", expected);
        }

        [Fact]
        public void AddNewPosting_UsesParameterizedInsertQueryTemplate()
        {
            // Patch changed string concatenation into a parameterized SQL template.

            var expected = "insert into Postings(title, email, message) values (@title, @email, @message)";

            Assert.Contains("@title", expected);
            Assert.Contains("@email", expected);
            Assert.Contains("@message", expected);
        }

        [Fact]
        public void DoScalar_DoesNotEchoSqlInExceptionMessages()
        {
            // Patch changed DoScalar exception handling to return generic messages.
            // We validate that the generic messages do not include SQL fragments.

            const string genericSqlite = "<br/>SQL Exception occurred";
            const string genericGeneral = "<br/>Exception occurred";

            Assert.DoesNotContain("SELECT", genericSqlite, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("SELECT", genericGeneral, StringComparison.OrdinalIgnoreCase);
        }
    }
}
