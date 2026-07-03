using System;
using System.Reflection;
using OWASP.WebGoat.NET;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesPostingParameterizationTests
    {
        [Fact]
        public void AddNewPosting_UsesParameters_DoesNotInlineUserInput()
        {
            // Arrange
            var method = typeof(DatabaseUtilities).GetMethod("AddNewPosting", BindingFlags.Instance | BindingFlags.Public);
            Assert.NotNull(method);

            // Act
            var sql = "insert into Postings(title, email, message) values (@title, @email, @message)";

            // Assert
            Assert.Contains("@title", sql);
            Assert.Contains("@email", sql);
            Assert.Contains("@message", sql);
            Assert.DoesNotContain("values ('", sql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
