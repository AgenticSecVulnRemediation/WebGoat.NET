using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPaymentsParameterizationTests
    {
        [Fact]
        public void GetPayments_UsesParameterizedQuery_ForCustomerNumber()
        {
            // Arrange
            // Delta test for PR 4699: verify customerNumber is bound as a parameter in GetPayments.
            var sourcePath = System.IO.Path.Combine("WebGoat", "App_Code", "DB", "MySqlDbProvider.cs");

            // Act
            var src = System.IO.File.ReadAllText(sourcePath);

            // Assert
            Assert.Contains("select * from Payments where customerNumber = @customerNumber", src);
            Assert.Contains("cmd.Parameters.AddWithValue(\"@customerNumber\"", src);
            Assert.Contains("new MySqlDataAdapter(cmd)", src);

            // Previously vulnerable pattern
            Assert.DoesNotContain("select * from Payments where customerNumber = \" + customerNumber", src);
        }
    }
}
