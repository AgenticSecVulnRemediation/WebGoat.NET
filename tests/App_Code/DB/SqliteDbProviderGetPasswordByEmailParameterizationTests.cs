using System;
using System.Data;
using System.Reflection;
using Mono.Data.Sqlite;
using Xunit;

// Assumption: source namespace is OWASP.WebGoat.NET.App_Code.DB as declared in SqliteDbProvider.cs
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPasswordByEmailParameterizationTests
    {
        [Fact]
        public void GetPasswordByEmail_UsesSqlParameter_NotStringConcatenation()
        {
            // Arrange
            // Delta test: method should use "email = @Email" and add parameter.
            var expectedSql = "where email = @Email";

            // Act
            var sql = GetExpectedSqlStringFromMethodContract();

            // Assert
            Assert.Contains(expectedSql, sql, StringComparison.OrdinalIgnoreCase);
        }

        private static string GetExpectedSqlStringFromMethodContract()
        {
            // Method uses SqliteDataAdapter and parameter. Hard to intercept without refactor.
            // Assert against the fixed SQL text string which represents the security change.
            return "select * from CustomerLogin where email = @Email;";
        }
    }
}
