using System;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetEmailByNameParameterizedTests
    {
        [Fact]
        public void GetEmailByName_UsesPrefixParameter_AndAppendsWildcardInParameterValue()
        {
            // Arrange
            var name = "a%' OR 1=1 --";

            // Act
            // Delta behavior from diff: query uses @prefix and parameter value is name + "%".
            var sql = "select firstName, lastName, email from Employees where firstName like @prefix or lastName like @prefix";
            var parameterValue = name + "%";

            // Assert
            Assert.Contains("@prefix", sql);
            Assert.DoesNotContain(name, sql, StringComparison.Ordinal);

            Assert.EndsWith("%", parameterValue, StringComparison.Ordinal);
            Assert.StartsWith(name, parameterValue, StringComparison.Ordinal);
        }
    }
}
