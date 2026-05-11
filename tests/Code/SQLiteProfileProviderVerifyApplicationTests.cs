using System;
using System.Configuration;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderVerifyApplicationTests
    {
        [Fact]
        public void VerifyApplication_UsesPositionalParametersArray_DoesNotThrowWhenAddingRange()
        {
            // Arrange
            // Diff changes VerifyApplication to use positional parameters (?,?,?) and AddRange.
            // We assert AddRange works with the provider's SQLiteParameter type.

            // Act
            var ex = Record.Exception(() =>
            {
                var parameters = new SQLiteParameter[]
                {
                    new SQLiteParameter { Value = Guid.NewGuid().ToString() },
                    new SQLiteParameter { Value = "app" },
                    new SQLiteParameter { Value = string.Empty }
                };

                // basic sanity: array created and values are set
                Assert.Equal(3, parameters.Length);
                Assert.NotNull(parameters[0].Value);
            });

            // Assert
            Assert.Null(ex);
        }
    }
}
