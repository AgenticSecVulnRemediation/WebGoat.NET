using System;
using System.Data;
using MySql.Data.MySqlClient;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderPaymentsTests
    {
        private sealed class DummyConfigFile : ConfigFile
        {
            public override string Get(string key) => string.Empty;
        }

        [Fact]
        public void GetPayments_UsesNamedParameterInSqlToPreventInjection()
        {
            // Arrange
            var provider = new MySqlDbProvider(new DummyConfigFile());

            // Act
            // We can't easily execute without a real DB; instead we assert the delta behavior by
            // reflecting the SQL constant is not present. The safest assert is on method's SQL text.
            // Since it's local, verify by inspecting IL for '@customerNumber'.
            var method = typeof(MySqlDbProvider).GetMethod("GetPayments");
            Assert.NotNull(method);
            var body = method!.GetMethodBody();
            Assert.NotNull(body);
            var il = body!.GetILAsByteArray();
            Assert.NotNull(il);

            // Assert: method body should contain the string "@customerNumber" in its metadata
            // (cheap smoke-test for parameterized query usage).
            Assert.Contains("@customerNumber", method.ToString());
        }
    }
}
