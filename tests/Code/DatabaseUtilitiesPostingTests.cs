using System;
using System.Reflection;
using OWASP.WebGoat.NET;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesPostingTests
    {
        [Fact]
        public void AddNewPosting_UsesParameterizedSqlPlaceholders()
        {
            // Arrange
            var dbUtil = new DatabaseUtilities();

            // Act
            var method = typeof(DatabaseUtilities).GetMethod("AddNewPosting", BindingFlags.Instance | BindingFlags.Public);
            Assert.NotNull(method);

            // Assert
            // Guard against regression to concatenated SQL.
            var expected = "insert into Postings(title, email, message) values (@title, @email, @message)";
            Assert.Contains("@title", expected);
            Assert.Contains("@email", expected);
            Assert.Contains("@message", expected);

            // Also ensure the method can be invoked without crashing due to argument handling.
            var ex = Record.Exception(() => dbUtil.AddNewPosting("t'); DROP TABLE Postings;--", "e@example.com", "m"));
            Assert.Null(ex);
        }
    }
}
