using System;
using Xunit;
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesParameterizedQueriesTests
    {
        [Fact]
        public void DatabaseUtilities_MailingListQueries_AreParameterized()
        {
            // Arrange/Act
            var moduleBytes = System.IO.File.ReadAllBytes(typeof(DatabaseUtilities).Module.FullyQualifiedName);
            var moduleText = System.Text.Encoding.UTF8.GetString(moduleBytes);

            // Assert: verify the fixed parameterized forms exist.
            Assert.Contains("SELECT FirstName, LastName, Email FROM MailingList WHERE Email = @Email", moduleText);
            Assert.Contains("INSERT INTO mailinglist (firstname, lastname, email) VALUES (@First, @Last, @Email)", moduleText);
            Assert.Contains("@Email", moduleText);
            Assert.Contains("@First", moduleText);
            Assert.Contains("@Last", moduleText);
        }
    }
}
