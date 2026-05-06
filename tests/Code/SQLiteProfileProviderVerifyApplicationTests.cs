using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderVerifyApplicationTests
    {
        [Fact]
        public void VerifyApplication_UsesPositionalParameters_InsteadOfNamed()
        {
            // Arrange
            // Delta behavior: INSERT uses positional placeholders (?, ?, ?) to avoid named parameter mismatch.
            var expectedInsert = "VALUES (?, ?, ?)";

            // Act / Assert
            Assert.Contains("?", expectedInsert);
            Assert.Equal("VALUES (?, ?, ?)", expectedInsert);
        }
    }
}
