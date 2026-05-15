using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Web.Profile;
using Xunit;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderGetPropertyValuesFromDatabaseTests
    {
        [Fact]
        public void MembershipApplicationName_SetTooLong_ThrowsProviderException()
        {
            // Arrange
            var tooLong = new string('b', 257);

            // Act + Assert
            Assert.Throws<ProviderException>(() => SQLiteProfileProvider.MembershipApplicationName = tooLong);
        }
    }
}
