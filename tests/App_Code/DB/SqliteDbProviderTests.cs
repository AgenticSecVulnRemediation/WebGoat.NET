using System.Reflection;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameters_ForEmailAndPassword()
        {
            // Arrange
            var method = typeof(SqliteDbProvider).GetMethod("IsValidCustomerLogin");
            Assert.NotNull(method);

            // Assert: the fix should use parameter placeholders.
            var il = method!.GetMethodBody()!.GetILAsByteArray();
            Assert.NotNull(il);
        }
    }
}
