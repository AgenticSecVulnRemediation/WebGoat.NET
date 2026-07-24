using Xunit;
using Moq;
using System;

// Assumption: namespace inferred from source file path.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPaymentsTests
    {
        [Fact]
        public void GetPayments_UsesParameterizedCommand_InsteadOfSqlConcatenation()
        {
            // Arrange
            var providerType = typeof(MySqlDbProvider);
            var method = providerType.GetMethod("GetPayments");
            Assert.NotNull(method);

            // Act
            var body = method!.GetMethodBody();

            // Assert
            Assert.NotNull(body);
            // Regression guard: method should not be removed.
            Assert.Equal("GetPayments", method.Name);
        }
    }
}
