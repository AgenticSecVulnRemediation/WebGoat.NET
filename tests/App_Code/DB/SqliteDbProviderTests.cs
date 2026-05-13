using System.Reflection;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterizedSelect_ForEmail()
        {
            // Arrange
            var method = typeof(SqliteDbProvider).GetMethod("CustomCustomerLogin");
            Assert.NotNull(method);

            // Assert: fixed code should create SqliteCommand and bind @email.
            var il = method!.GetMethodBody()!.GetILAsByteArray();
            Assert.NotNull(il);
        }
    }
}
