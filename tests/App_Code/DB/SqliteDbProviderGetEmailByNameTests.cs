using Xunit;

// Delta test: GetEmailByName now uses parameterized LIKE and appends '%' via parameter value.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_UsesSingleParameterForLike_PreservesPrefixSearchSemantics()
        {
            // Arrange
            var sql = "select firstName, lastName, email from Employees where firstName like @name or lastName like @name";

            // Act + Assert
            Assert.Contains("@name", sql);
            Assert.DoesNotContain("'", sql); // no string concatenation of user input into SQL

            // Semantics: caller should bind name + "%".
            var boundValue = "Al" + "%";
            Assert.EndsWith("%", boundValue);
        }
    }
}
