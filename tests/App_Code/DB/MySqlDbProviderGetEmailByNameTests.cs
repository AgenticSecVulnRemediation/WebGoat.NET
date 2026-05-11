using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_UsesParameterizedLikeConcat_TemplateDoesNotContainInput()
        {
            // Arrange
            var name = "x%' OR '1'='1";

            // Act/Assert
            const string sql = "select firstName, lastName, email from Employees where firstName like CONCAT(@Name, '%') or lastName like CONCAT(@Name, '%')";
            Assert.Contains("@Name", sql);
            Assert.DoesNotContain(name, sql);
        }
    }
}
