using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderEmailSearchTests
    {
        [Fact]
        public void GetEmailByName_MethodExists_AcceptsStringParameter()
        {
            // Delta behavior: LIKE query now parameterized (@name).
            var method = typeof(MySqlDbProvider).GetMethod("GetEmailByName");
            Assert.NotNull(method);
            var parameters = method!.GetParameters();
            Assert.Single(parameters);
            Assert.Equal(typeof(string), parameters[0].ParameterType);
        }
    }
}
