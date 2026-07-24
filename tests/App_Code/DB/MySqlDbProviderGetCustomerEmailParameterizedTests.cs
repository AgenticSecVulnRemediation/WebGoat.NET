using System;
using Xunit;

// Assumption: MySqlDbProvider exists in OWASP.WebGoat.NET.App_Code.DB
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailParameterizedTests
    {
        [Fact]
        public void GetCustomerEmail_Signature_IsString_ReturnsString()
        {
            var mi = typeof(MySqlDbProvider).GetMethod("GetCustomerEmail");
            Assert.NotNull(mi);

            var parameters = mi!.GetParameters();
            Assert.Single(parameters);
            Assert.Equal(typeof(string), parameters[0].ParameterType);
            Assert.Equal(typeof(string), mi.ReturnType);
        }
    }
}
