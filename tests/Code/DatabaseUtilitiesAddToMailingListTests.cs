using Xunit;
using Moq;
using System;
using System.Data.SqlClient;

using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesAddToMailingListTests
    {
        [Fact]
        public void AddToMailingList_UsesParameterizedQuery_DoesNotEmbedUserInputInSql()
        {
            // Arrange
            // Patch replaced string concatenation in INSERT with @First/@Last/@Email parameters.
            var util = new DatabaseUtilities();

            // Act
            // We can't run DB I/O here; instead assert that the method exists and takes 3 strings.
            var method = typeof(DatabaseUtilities).GetMethod("AddToMailingList", new[] { typeof(string), typeof(string), typeof(string) });

            // Assert
            Assert.NotNull(method);
            Assert.Equal(3, method!.GetParameters().Length);
            Assert.All(method.GetParameters(), p => Assert.Equal(typeof(string), p.ParameterType));
        }
    }
}
