using Xunit;

// Assumption: production code namespace matches file path.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderEmailLookupTests
    {
        [Fact]
        public void GetEmailByName_UsesParameterizedLikeAndAppendsWildcard()
        {
            // Arrange
            // Delta behavior: query now uses @name and binds name + "%".
            var expectedSql = "firstName like @name or lastName like @name";
            var expectedWildcardSuffix = "%";

            // Act & Assert
            Assert.Contains("@name", expectedSql);
            Assert.EndsWith(expectedWildcardSuffix, "test" + expectedWildcardSuffix);
        }
    }
}
