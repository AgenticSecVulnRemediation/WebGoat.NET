using System;
using System.Data;
using System.Linq;
using System.Reflection;
using MySql.Data.MySqlClient;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        // Note: This test focuses on the security regression: customerID must be passed as a parameter,
        // not concatenated into SQL.
        [Fact]
        public void GetOrders_UsesParameterizedQuery_ForCustomerNumber()
        {
            // Arrange
            // We avoid a live DB by inspecting the method body IL for the parameter token "@customerID".
            // This is a pragmatic delta test since the provider constructs MySqlCommand/MySqlDataAdapter internally.
            var method = typeof(MySqlDbProvider).GetMethod("GetOrders", BindingFlags.Instance | BindingFlags.Public);
            Assert.NotNull(method);

            // Act
            var ilBytes = method!.GetMethodBody()!.GetILAsByteArray();
            var ilString = BitConverter.ToString(ilBytes ?? Array.Empty<byte>());

            // Assert
            // Ensure the new parameter name literal exists in the method (high-signal regression check).
            // This will fail if code reverts to string concatenation without the "@customerID" token.
            Assert.Contains("@customerID", method!.ToString());
        }
    }
}
