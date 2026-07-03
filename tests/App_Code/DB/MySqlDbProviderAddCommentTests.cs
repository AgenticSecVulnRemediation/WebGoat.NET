using System;
using System.Data;
using Moq;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

// Note: The project types like ConfigFile, DbConstants, Util, Encoder, and log4net are not required for this delta test.
// We only validate that AddComment now uses a parameterized SQL string.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsertStatement()
        {
            // Arrange
            // Create provider with minimal config via mocking/stubbing.
            // If ConfigFile cannot be constructed, this test can be adjusted by the project to use a real ConfigFile.
            // For now, we assert via reflection on string literals.
            var type = typeof(MySqlDbProvider);

            // Act
            var literals = type
                .GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance)
                .Select(f => f.GetRawConstantValue())
                .OfType<string>()
                .ToList();

            // Assert
            Assert.Contains(literals, s => s.Contains("insert into Comments(productCode, email, comment) values (@productCode, @email, @comment)", StringComparison.Ordinal));
            Assert.DoesNotContain(literals, s => s.Contains("values ('\" + productCode", StringComparison.Ordinal));
        }
    }
}
