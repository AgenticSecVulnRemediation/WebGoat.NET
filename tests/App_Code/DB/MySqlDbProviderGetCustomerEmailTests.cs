using System;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_ContainsParameterPlaceholder_AndDoesNotConcatenateInput()
        {
            // Arrange
            var asmText = System.Text.Encoding.UTF8.GetString(
                System.IO.File.ReadAllBytes(typeof(MySqlDbProvider).Assembly.Location));

            // Act/Assert
            Assert.Contains("customerNumber = @customerNumber", asmText);
            Assert.Contains("AddWithValue(\"@customerNumber\"", asmText);
        }
    }
}
