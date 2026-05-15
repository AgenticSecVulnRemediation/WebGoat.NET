using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Web.Profile;
using Xunit;

// Assumption: source namespace and class exist as in file path WebGoat/Code/SQLiteProfileProvider.cs
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void ApplicationName_SetTooLong_ThrowsProviderException()
        {
            // Arrange
            var provider = new SQLiteProfileProvider();
            var tooLong = new string('a', 257);

            // Act + Assert
            Assert.Throws<ProviderException>(() => provider.ApplicationName = tooLong);
        }
    }
}
