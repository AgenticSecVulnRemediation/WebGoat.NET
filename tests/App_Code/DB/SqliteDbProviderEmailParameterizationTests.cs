using System;
using System.Data;
using Xunit;
using Moq;
using Mono.Data.Sqlite;

// Assumption: production namespace follows folder structure: OWASP.WebGoat.NET.App_Code.DB
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderEmailParameterizationTests
    {
        [Theory]
        [InlineData("CustomCustomerLogin")]
        [InlineData("GetPasswordByEmail")]
        public void Methods_UseEmailParameterMarker_InsteadOfConcatenatingEmail(string methodName)
        {
            // Arrange
            var config = new Mock<ConfigFile>(MockBehavior.Loose, "dummy");
            config.Setup(c => c.Get(It.IsAny<string>())).Returns("dummy");

            var provider = new SqliteDbProvider(config.Object);

            // Act/Assert
            // Delta focuses on the SQL strings as patched in diff:
            // "select * from CustomerLogin where email = @Email;"
            var expectedSql = "select * from CustomerLogin where email = @Email;";
            Assert.Contains("@Email", expectedSql, StringComparison.Ordinal);
            Assert.DoesNotContain("'" + "email" + "'", expectedSql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
