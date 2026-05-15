using Xunit;
using System.IO;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSetPropertyValuesTests
    {
        [Fact]
        public void SetPropertyValues_UsesPositionalParameters_AndDoesNotUseNamedDollarParameters()
        {
            // Arrange
            var source = File.ReadAllText("WebGoat/Code/SQLiteProfileProvider.cs");

            // Act & Assert
            // Delta asserts new use of '?' positional parameters for UPDATE/INSERT in SetPropertyValues.
            Assert.Contains("SET PropertyNames = ?, PropertyValuesString = ?, PropertyValuesBinary = ?, LastUpdatedDate = ? WHERE UserId = ?", source);
            Assert.Contains("VALUES (?, ?, ?, ?, ?)", source);
            Assert.DoesNotContain("$PropertyNames", source);
            Assert.DoesNotContain("$PropertyValuesString", source);
        }
    }
}
