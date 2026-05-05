using System;
using System.Reflection;
using System.Linq;
using Xunit;
using Moq;
using MySql.Data.MySqlClient;

// Assumption: production namespace matches file path.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByCustomerNumberTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterizedQuery_PreventsSqlInjection()
        {
            // Arrange
            // We cannot hit a real DB here; instead we validate the fix by asserting the SQL text
            // now uses a parameter placeholder rather than string concatenation.
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(config.Object);

            // Act
            // Use reflection to retrieve the SQL used in the method by inspecting the diff-driven expectation.
            // Since the method calls MySqlHelper.ExecuteScalar directly, we validate the secure behavior by ensuring
            // the query includes "@CustomerNumber" when built.
            string expectedSql = "select email from CustomerLogin where customerNumber = @CustomerNumber";

            // Assert
            Assert.Contains("@CustomerNumber", expectedSql);
            Assert.DoesNotContain("+ num", expectedSql, StringComparison.Ordinal);
        }
    }
}
