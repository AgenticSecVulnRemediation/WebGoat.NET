using System;
using Xunit;

// NOTE: Namespace inferred from file path and source.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetEmailByCustomerNumberParameterizationTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesNamedParameter_NotConcatenation()
        {
            // Arrange
            // Delta test for PR 4695: sqlite query now uses @num parameter.
            var sourcePath = System.IO.Path.Combine("WebGoat", "App_Code", "DB", "SqliteDbProvider.cs");

            // Act
            var src = System.IO.File.ReadAllText(sourcePath);

            // Assert
            Assert.Contains("select email from CustomerLogin where customerNumber = @num", src);
            Assert.Contains("cmd.Parameters.AddWithValue(\"@num\"", src);
            Assert.DoesNotContain("customerNumber = \" + num", src);
        }
    }
}
