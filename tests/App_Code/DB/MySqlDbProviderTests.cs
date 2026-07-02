using System;
using System.Linq;
using System.Reflection;
using Moq;
using Xunit;

// Note: Namespace inferred from source file.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterizedQuery_InsteadOfStringConcatenation()
        {
            // Arrange
            // Regression test for SQL injection fix: query now uses @customerNumber parameter.
            // The legacy code constructs ADO.NET objects directly; we assert the command text embedded in assembly.

            var asmText = System.Text.Encoding.UTF8.GetString(
                System.IO.File.ReadAllBytes(typeof(MySqlDbProvider).Assembly.Location));

            // Act/Assert
            Assert.Contains("select email from CustomerLogin where customerNumber = @customerNumber", asmText);
            Assert.DoesNotContain("select email from CustomerLogin where customerNumber = \" + customerNumber", asmText);
        }

        [Fact]
        public void UpdateCustomerPassword_UsesParameters_ForPasswordAndCustomerNumber()
        {
            // Arrange
            var asmText = System.Text.Encoding.UTF8.GetString(
                System.IO.File.ReadAllBytes(typeof(MySqlDbProvider).Assembly.Location));

            // Act/Assert
            Assert.Contains("UPDATE CustomerLogin SET password = @password WHERE customerNumber = @customerNumber", asmText);
            Assert.Contains("AddWithValue(\"@password\"", asmText);
            Assert.Contains("AddWithValue(\"@customerNumber\"", asmText);
            Assert.DoesNotContain("update CustomerLogin set password = '\" +", asmText);
        }

        [Fact]
        public void GetPayments_UsesParameterizedQuery_ForCustomerNumber()
        {
            // Arrange
            var asmText = System.Text.Encoding.UTF8.GetString(
                System.IO.File.ReadAllBytes(typeof(MySqlDbProvider).Assembly.Location));

            // Act/Assert
            Assert.Contains("select * from Payments where customerNumber = @customerNumber", asmText);
            Assert.Contains("new MySqlDataAdapter(cmd)", asmText);
            Assert.DoesNotContain("select * from Payments where customerNumber = \" + customerNumber", asmText);
        }
    }
}
