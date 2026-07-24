using Xunit;
using System;

using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesParameterBindingTests
    {
        [Fact]
        public void GetEmailByUserId_UsesPlaceholder_NotStringConcatenation()
        {
            // Arrange
            var attackerSupplied = "' OR 1=1 --";

            // Act
            var query = "SELECT Email FROM UserList WHERE UserID = @UserID";

            // Assert
            Assert.Contains("@UserID", query, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain(attackerSupplied, query, StringComparison.Ordinal);
            Assert.DoesNotContain("'" + attackerSupplied + "'", query, StringComparison.Ordinal);
        }

        [Fact]
        public void GetMailingListInfoByEmailAddress_UsesPlaceholder_NotStringConcatenation()
        {
            // Arrange
            var attackerEmail = "a@b.com' OR 1=1 --";

            // Act
            var query = "SELECT FirstName, LastName, Email FROM MailingList WHERE Email = @Email";

            // Assert
            Assert.Contains("@Email", query, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain(attackerEmail, query, StringComparison.Ordinal);
        }
    }
}
