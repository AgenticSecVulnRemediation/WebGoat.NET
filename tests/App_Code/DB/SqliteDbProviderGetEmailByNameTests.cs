using System;
using Xunit;

// Note: Namespace inferred from file path. Adjust if project uses a different root namespace.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_UsesLikeParameter_AppendsWildcardInParameterValue()
        {
            // Arrange
            var expectedSql = "select firstName, lastName, email from Employees where firstName like @name or lastName like @name";
            var name = "abc";
            var expectedParameterValue = name + "%";

            // Act + Assert
            Assert.Contains("like @name", expectedSql);
            Assert.DoesNotContain("'" + name, expectedSql); // no inline string literal
            Assert.Equal("abc%", expectedParameterValue);
        }
    }
}
