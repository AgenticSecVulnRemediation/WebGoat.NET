using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Web.Profile;
using Xunit;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderParameterOrderTests
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
