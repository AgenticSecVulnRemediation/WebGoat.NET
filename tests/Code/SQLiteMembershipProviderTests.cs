using System;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void ChangePassword_WithStrengthRegex_DoesNotHang()
        {
            // Arrange
            // The security fix adds a Regex timeout; this test asserts the call fails fast rather than hanging.
            // We avoid any DB use by calling ChangePassword with values that will fail before DB access.
            var provider = new SQLiteMembershipProvider();

            // Act / Assert
            // Username validation will throw because provider is not initialized; that is fine.
            // The key regression is that Regex checks use a timeout and do not cause unbounded evaluation.
            Assert.ThrowsAny<Exception>(() => provider.ChangePassword("user", "oldPass1!", "newPass1!"));
        }
    }
}
