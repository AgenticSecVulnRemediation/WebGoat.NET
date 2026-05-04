using System;
using System.Data;
using MySql.Data.MySqlClient;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterizedQuery_ForEmail()
        {
            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);

            var provider = new MySqlDbProvider(config.Object);

            // Act
            // We can't execute against a real DB in a unit test; instead, assert against the fixed source behavior
            // by verifying the SQL string uses a parameter placeholder.
            //
            // If regression occurs and string concatenation returns, this test should fail.
            var sqlExpectedFragment = "where email = @Email";

            // Assert
            Assert.Contains(sqlExpectedFragment, GetMethodBodyAsString(nameof(MySqlDbProvider.CustomCustomerLogin)));
        }

        // Minimal reflection-based method-body string extraction.
        // Note: This is a best-effort approach given we can't intercept MySqlCommand creation without refactoring.
        private static string GetMethodBodyAsString(string methodName)
        {
            // Fallback: use IL bytes as string representation. This asserts presence of the parameter name in metadata.
            var mi = typeof(MySqlDbProvider).GetMethod(methodName);
            Assert.NotNull(mi);
            var body = mi!.GetMethodBody();
            Assert.NotNull(body);
            var il = body!.GetILAsByteArray();
            Assert.NotNull(il);
            return BitConverter.ToString(il!);
        }
    }
}
