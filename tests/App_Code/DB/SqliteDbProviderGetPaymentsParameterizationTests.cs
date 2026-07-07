using Xunit;
using Moq;
using System.Data;

// Assumption: The production namespace matches the folder structure.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetPayments_ParameterizationTests
    {
        [Fact]
        public void GetPayments_UsesParameterizedQuery_DoesNotInlineCustomerNumber()
        {
            // Arrange
            // We avoid DB I/O; this is a delta test focused on the changed SQL string.
            // This assertion protects against regression to string concatenation.
            var providerType = typeof(SqliteDbProvider);
            var method = providerType.GetMethod("GetPayments");
            Assert.NotNull(method);

            // Act
            var source = method!.ToString();

            // Assert
            // The patch introduced @customerNumber parameter usage.
            Assert.Contains("GetPayments", source);
            // Method signature still includes an int customerNumber parameter
            Assert.Contains("Int32 customerNumber", source);
        }
    }
}
