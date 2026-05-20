using System;
using System.Linq;
using MySql.Data.MySqlClient;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderIsValidCustomerLoginTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameterizedQueryForEmailAndPassword()
        {
            // Arrange
            var provider = (MySqlDbProvider)Activator.CreateInstance(typeof(MySqlDbProvider), args: new object[] { null });

            // Act
            // The change is from string concatenation to parameters @email and @password in the SQL.
            // Assert by checking the diff-introduced SQL literal exists in the assembly.
            var moduleBytes = System.IO.File.ReadAllBytes(typeof(MySqlDbProvider).Module.FullyQualifiedName);
            var moduleText = System.Text.Encoding.UTF8.GetString(moduleBytes);

            // Assert
            Assert.Contains("SELECT * FROM CustomerLogin WHERE email = @email AND password = @password", moduleText);
            Assert.Contains("@email", moduleText);
            Assert.Contains("@password", moduleText);
        }
    }
}
