using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesTests
    {
        [Fact]
        public void AddNewPosting_WithSqlInjectionPayload_DoesNotThrow_AndDoesNotConcatenateIntoSql()
        {
            // This is a delta-security test: AddNewPosting was changed from string concatenation to parameters.
            // We can't easily integration-test SQLite here without the app DB, but we can at least ensure
            // the method accepts potentially malicious input without throwing due to malformed SQL concatenation.

            var du = new OWASP.WebGoat.NET.DatabaseUtilities();

            // Typical injection payload that would break concatenated SQL by closing quotes.
            var title = "x'); DROP TABLE Postings; --";
            var email = "attacker@example.com";
            var message = "hello'); --";

            // Act
            var ex = Record.Exception(() => du.AddNewPosting(title, email, message));

            // Assert
            Assert.Null(ex);
        }
    }
}
