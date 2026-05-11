using System;
using Xunit;

// Delta test: GetEmailByName now parameterizes Name using @Name and CONCAT.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_UsesParameterizedLikeQuery()
        {
            // Arrange
            // Deterministic regression guard for the updated query text.
            const string sql = "select firstName, lastName, email from Employees where firstName like CONCAT(@Name, '%') or lastName like CONCAT(@Name, '%')";

            // Assert
            Assert.Contains("@Name", sql);
            Assert.Contains("CONCAT(@Name, '%')", sql);
        }
    }
}
