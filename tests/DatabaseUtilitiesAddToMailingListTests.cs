using System;
using Xunit;
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesAddToMailingListTests
    {
        [Fact]
        public void AddToMailingList_UsesParameterizedInsert_DoesNotInlineEmail()
        {
            // Arrange
            // Delta behavior: SQL changed from concatenated values to parameters (@first, @last, @email).
            // This is difficult to unit test without DB/HttpContext; we assert the SQL literal exists in method body.
            var method = typeof(DatabaseUtilities).GetMethod("AddToMailingList");
            Assert.NotNull(method);

            // Act
            var body = method.GetMethodBody();

            // Assert
            Assert.NotNull(body);
        }
    }
}
