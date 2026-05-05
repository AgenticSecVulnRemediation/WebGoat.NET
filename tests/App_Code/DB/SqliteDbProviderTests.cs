using System;
using Mono.Data.Sqlite;
using Moq;
using Xunit;

// Assumption: production namespace matches source file.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsert_NotConcatenatedSql()
        {
            // Arrange
            // Security fix: insert query moved from string concatenation to parameterized query.
            // This test asserts that the updated SQL template contains named parameters.
            var method = typeof(SqliteDbProvider).GetMethod("AddComment");

            // Assert
            Assert.NotNull(method);
            Assert.True(ContainsUserString(method!.Module, "insert into Comments(productCode, email, comment) values (@productCode, @Email, @Comment);"),
                "Expected parameterized insert with @productCode, @Email, @Comment");
        }

        private static bool ContainsUserString(Module module, string expected)
        {
            try
            {
                var location = module.Assembly.Location;
                if (string.IsNullOrEmpty(location))
                    return false;

                var bytes = System.IO.File.ReadAllBytes(location);
                var text = System.Text.Encoding.UTF8.GetString(bytes);
                return text.Contains(expected, StringComparison.Ordinal);
            }
            catch
            {
                return false;
            }
        }
    }
}
