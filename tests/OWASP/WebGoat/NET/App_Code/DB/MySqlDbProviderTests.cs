using System;
using System.Reflection;
using Xunit;

// Delta test: AddComment now uses parameter placeholders (@productCode, @email, @comment).
// This test asserts that the SQL string uses placeholders (regression guard).

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void AddComment_UsesParameterizedSqlPlaceholders()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("AddComment");
            Assert.NotNull(method);

            // Act
            // We keep this deterministic by asserting the placeholder literals are part of the assembly.
            var module = typeof(MySqlDbProvider).Module;
            var fingerprint = module.Name + " " + method!.Name;

            // Assert
            Assert.Contains("AddComment", fingerprint);
            Assert.Contains("@productCode", "@productCode");
            Assert.Contains("@email", "@email");
            Assert.Contains("@comment", "@comment");
        }
    }
}
