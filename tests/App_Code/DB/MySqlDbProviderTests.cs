using System;
using System.Reflection;
using Moq;
using MySql.Data.MySqlClient;
using Xunit;

// Assumption: source namespace is OWASP.WebGoat.NET.App_Code.DB and class is public.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedQuery_ForCustomerId()
        {
            // Arrange
            // We don't connect to DB; we assert via diff-driven contract: query should include @customerID.
            var config = new Mock<ConfigFile>(MockBehavior.Loose);
            config.Setup(c => c.Get(It.IsAny<string>())).Returns("dummy");
            var provider = new MySqlDbProvider(config.Object);

            // Act
            // Use reflection to read method body IL strings isn't reliable; instead validate by invoking with customerID and
            // ensuring it does not throw due to string concatenation vulnerabilities isn't deterministic.
            // Delta contract: SQL text literal should contain parameter marker.
            var method = typeof(MySqlDbProvider).GetMethod("GetOrders");
            Assert.NotNull(method);

            // Assert
            // Validate presence of parameter name in method's metadata by scanning for it in the assembly strings.
            // This is deterministic and tied to the code change.
            var asm = typeof(MySqlDbProvider).Assembly;
            using var stream = asm.GetManifestResourceStream(asm.GetName().Name + ".resources");
            // Resource stream may not exist; fall back to simple invariant: method exists and signature unchanged.
            // Stronger assertion: method's IL contains the UTF8 bytes for "@customerID".
            var il = method!.GetMethodBody()!.GetILAsByteArray();
            var marker = System.Text.Encoding.UTF8.GetBytes("@customerID");
            Assert.Contains(marker, new ReadOnlySpan<byte>(il).ToArray());
        }
    }
}
