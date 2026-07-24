using System;
using System.Reflection;
using Moq;
using Xunit;

// NOTE: Namespace inferred from file path and source.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailParameterizationTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterizedQuery_DoesNotInlineCustomerNumber()
        {
            // Arrange
            // This delta test verifies the vulnerable concatenation was replaced with a parameter.
            // Because MySqlCommand is instantiated directly, we assert on the SQL text in the source
            // as a regression test for the fix.
            var sourcePath = System.IO.Path.Combine("WebGoat", "App_Code", "DB", "MySqlDbProvider.cs");

            // Act
            var src = System.IO.File.ReadAllText(sourcePath);

            // Assert
            Assert.Contains("select email from CustomerLogin where customerNumber = @customerNumber", src);
            Assert.Contains("Parameters.AddWithValue(\"@customerNumber\"", src);

            // Ensure the old concatenation pattern isn't present in the GetCustomerEmail method region
            // (best-effort string check)
            Assert.DoesNotContain("customerNumber = \" + customerNumber", src);
        }
    }
}
