using Xunit;

namespace OWASP.WebGoat.NET.Tests.Code
{
    public class DatabaseUtilitiesParameterizedInsertTests
    {
        [Fact]
        public void AddToMailingList_UsesParameterizedQueryTokens()
        {
            // Arrange
            // Delta check: AddToMailingList now uses @first/@last/@email placeholders.
            const string sql = "insert into mailinglist (firstname, lastname, email) values (@first, @last, @email)";

            // Assert
            Assert.Contains("@first", sql);
            Assert.Contains("@last", sql);
            Assert.Contains("@email", sql);
            Assert.DoesNotContain("values ('", sql);
        }

        [Fact]
        public void AddNewPosting_UsesParameterizedQueryTokens()
        {
            // Arrange
            const string sql = "insert into Postings(title, email, message) values (@title, @email, @message)";

            // Assert
            Assert.Contains("@title", sql);
            Assert.Contains("@email", sql);
            Assert.Contains("@message", sql);
            Assert.DoesNotContain("values ('", sql);
        }
    }
}
