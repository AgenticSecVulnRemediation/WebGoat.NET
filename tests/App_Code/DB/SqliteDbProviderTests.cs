using System;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterizedQuery_WithAtCustomerNumber()
        {
            // Arrange
            var asmText = System.Text.Encoding.UTF8.GetString(
                System.IO.File.ReadAllBytes(typeof(SqliteDbProvider).Assembly.Location));

            // Act/Assert
            Assert.Contains("select email from CustomerLogin where customerNumber = @customerNumber", asmText);
            Assert.Contains("AddWithValue(\"@customerNumber\"", asmText);
            Assert.DoesNotContain("select email from CustomerLogin where customerNumber = \" + customerNumber", asmText);
        }
    }
}
