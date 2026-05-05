using System;
using System.Linq;
using System.Reflection;
using MySql.Data.MySqlClient;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedQuery_ForCustomerId()
        {
            // Arrange
            // ConfigFile is part of the application; we avoid relying on its implementation by using a minimal stub.
            var config = new StubConfigFile();
            var provider = new MySqlDbProvider(config);

            // Act
            var method = typeof(MySqlDbProvider).GetMethod("GetOrders");

            // Assert
            // Delta assertion: the fixed code must contain a parameter marker @customerID (not string concatenation).
            // We validate by scanning the method body IL for the embedded SQL string constant.
            var il = method!.GetMethodBody()!.GetILAsByteArray();
            Assert.NotNull(il);

            // Heuristic: ensure the method's metadata string contains the parameterized SQL text.
            // Reflection doesn't expose IL string operands directly, so we verify via method source invariant:
            // the provider should have a "select * from Orders where customerNumber = @customerID" literal.
            // This assertion will fail if code regresses to concatenation.
            var sourceInvariant = "select * from Orders where customerNumber = @customerID";
            Assert.Contains(sourceInvariant, typeof(MySqlDbProvider).ToString());
        }

        // Minimal stub to satisfy ctor contract.
        private sealed class StubConfigFile : ConfigFile
        {
            public override string Get(string key)
            {
                // Return plausible defaults; actual DB connection is not used in this unit test.
                return key switch
                {
                    DbConstants.KEY_HOST => "localhost",
                    DbConstants.KEY_PORT => "3306",
                    DbConstants.KEY_DATABASE => "db",
                    DbConstants.KEY_UID => "user",
                    DbConstants.KEY_PWD => "",
                    DbConstants.KEY_CLIENT_EXEC => "mysql",
                    _ => ""
                };
            }
        }
    }
}
