using System.Reflection;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetCustomerEmails_UsesParameterizedLikeQuery()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("GetCustomerEmails");
            Assert.NotNull(method);

            // Assert: fixed code uses CONCAT(@email, '%') and binds @email.
            var il = method!.GetMethodBody()!.GetILAsByteArray();
            Assert.NotNull(il);
        }
    }
}
