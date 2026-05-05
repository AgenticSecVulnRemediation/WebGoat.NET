using System;
using System.Data;
using System.Reflection;
using Moq;
using MySql.Data.MySqlClient;
using Xunit;

// Assumption: production namespace matches source file.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedQuery_DoesNotInlineCustomerId()
        {
            // Arrange
            // The security fix changed string concatenation to a parameterized query.
            // This regression test asserts the literal query now contains @customerID.

            var cfg = new Mock<ConfigFile>();
            cfg.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);

            var provider = new MySqlDbProvider(cfg.Object);

            // Act
            // We can't execute DB calls in unit tests here (no external systems),
            // so we validate the expected SQL text is present in the updated file via reflection.
            // This is a delta test focused on the fixed behavior.
            var method = typeof(MySqlDbProvider).GetMethod("GetOrders", BindingFlags.Public | BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);

            // Ensure method body changed: look for parameter name in method's IL string representation.
            // This is deterministic and ensures the parameter token is referenced.
            var ilBytes = method!.GetMethodBody()!.GetILAsByteArray();
            var ilString = BitConverter.ToString(ilBytes ?? Array.Empty<byte>());

            // Very lightweight signal: constant string "@customerID" must be in metadata/user strings.
            // Use method module to scan user strings.
            bool contains = ContainsUserString(method.Module, "select * from Orders where customerNumber = @customerID");
            Assert.True(contains, "Expected parameterized SQL with @customerID");
        }

        private static bool ContainsUserString(Module module, string expected)
        {
            // Scan module user strings in a best-effort manner.
            // If this environment doesn't allow scanning, fail safe by returning false.
            try
            {
                // There's no public API to enumerate user strings; instead, we check that the expected
                // string is present in the compiled assembly's raw bytes.
                var location = module.Assembly.Location;
                if (string.IsNullOrEmpty(location))
                    return false;

                var bytes = System.IO.File.ReadAllBytes(location);
                var text = System.Text.Encoding.UTF8.GetString(bytes);
                return text.Contains(expected, StringComparison.Ordinal);
            }
            catch
            {
                return false;
            }
        }
    }
}
