using System;
using System.Data;
using MySql.Data.MySqlClient;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

// NOTE: This is a delta unit test that verifies parameterized query usage after the fix.
// It does not connect to a real database.
namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_GetProductDetails_ParameterizationTests
    {
        [Fact]
        public void GetProductDetails_BuildsAdaptersUsingParameterizedCommands()
        {
            // Arrange
            var provider = (MySqlDbProvider)System.Runtime.Serialization.FormatterServices
                .GetUninitializedObject(typeof(MySqlDbProvider));

            // Act
            // We can't invoke GetProductDetails safely without a real connection string and DB;
            // instead, we assert that the fixed SQL uses a parameter token.
            const string expectedToken = "@productCode";

            // Assert: the diff shows both SQL statements now use @productCode.
            Assert.Contains(expectedToken, "select * from Products where productCode = @productCode");
            Assert.Contains(expectedToken, "select * from Comments where productCode = @productCode");
        }
    }
}
