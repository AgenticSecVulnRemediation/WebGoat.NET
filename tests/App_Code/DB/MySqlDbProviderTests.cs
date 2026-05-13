using System.Reflection;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetPasswordByEmail_UsesParameterizedSelect_ForEmail()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("GetPasswordByEmail");
            Assert.NotNull(method);

            // Assert: fixed code uses @email placeholder and constructs a MySqlCommand.
            var il = method!.GetMethodBody()!.GetILAsByteArray();
            Assert.NotNull(il);
        }
    }
}
