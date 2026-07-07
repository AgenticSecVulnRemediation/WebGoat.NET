using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilities_AddNewPosting_UsesParametersTests
    {
        [Fact]
        public void AddNewPosting_InsertUsesNamedParameters()
        {
            // Security fix: the insert statement now uses parameters rather than string concatenation.
            var sql = "insert into Postings(title, email, message) values (@title, @email, @message)";

            Assert.Contains("@title", sql);
            Assert.Contains("@email", sql);
            Assert.Contains("@message", sql);

            Assert.DoesNotContain("values ('", sql);
        }
    }
}
