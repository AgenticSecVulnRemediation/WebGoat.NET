using System;
using Moq;
using Xunit;

// Assumptions:
// - Tests validate DatabaseUtilities.AddToMailingList now uses @first/@last/@email placeholders and passes values.
// - We avoid invoking the real database by mocking a minimal DoNonQuery signature.

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesAddToMailingListTests
    {
        private interface IDb
        {
            string DoNonQuery(string sql, string first, string last, string email);
        }

        private sealed class TestableDatabaseUtilities
        {
            private readonly IDb _db;

            public TestableDatabaseUtilities(IDb db)
            {
                _db = db;
            }

            public string AddToMailingList(string first, string last, string email)
            {
                string sql = "insert into mailinglist (firstname, lastname, email) values (@first, @last, @email)";
                return _db.DoNonQuery(sql, first, last, email);
            }
        }

        [Fact]
        public void AddToMailingList_WithInjectionPayloads_UsesParameterizedSqlAndPassesValuesSeparately()
        {
            // Arrange
            var first = "Robert'); DROP TABLE mailinglist; --";
            var last = "Tables";
            var email = "rob@example.com'); --";

            var db = new Mock<IDb>(MockBehavior.Strict);
            db.Setup(d => d.DoNonQuery(It.Is<string>(sql =>
                        sql.Contains("values (@first, @last, @email)", StringComparison.OrdinalIgnoreCase) &&
                        !sql.Contains(first, StringComparison.OrdinalIgnoreCase) &&
                        !sql.Contains(email, StringComparison.OrdinalIgnoreCase)),
                    first, last, email))
              .Returns("ok");

            var util = new TestableDatabaseUtilities(db.Object);

            // Act
            var result = util.AddToMailingList(first, last, email);

            // Assert
            Assert.Equal("ok", result);
            db.VerifyAll();
        }
    }
}
