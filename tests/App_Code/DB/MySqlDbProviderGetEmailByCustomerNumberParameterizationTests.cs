using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByCustomerNumberParameterizationTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterizedExecuteScalar()
        {
            // Arrange
            // Delta test for PR 4696: ExecuteScalar now uses @num parameter rather than concatenation.
            var sourcePath = System.IO.Path.Combine("WebGoat", "App_Code", "DB", "MySqlDbProvider.cs");

            // Act
            var src = System.IO.File.ReadAllText(sourcePath);

            // Assert
            Assert.Contains("select email from CustomerLogin where customerNumber = @num", src);
            Assert.Contains("new MySqlParameter(\"@num\"", src);

            // Previously vulnerable concatenation
            Assert.DoesNotContain("customerNumber = \" + num", src);
        }
    }
}
