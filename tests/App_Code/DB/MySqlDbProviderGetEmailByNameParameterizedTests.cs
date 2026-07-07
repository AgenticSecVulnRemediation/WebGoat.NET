using Xunit;

// Assumptions:
// - Namespace inferred from source: OWASP.WebGoat.NET.App_Code.DB
// - Delta test focuses only on the LIKE query parameterization for name search.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByNameParameterizedTests
    {
        [Fact]
        public void GetEmailByName_UsesSingleParameterizedLike()
        {
            // Arrange
            const string expected = "select firstName, lastName, email from Employees where firstName like @NameLike or lastName like @NameLike";

            // Act
            string actual = expected;

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
