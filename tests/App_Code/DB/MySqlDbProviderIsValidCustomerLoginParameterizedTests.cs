using System;
using Xunit;

// Assumption: MySqlDbProvider exists in OWASP.WebGoat.NET.App_Code.DB
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderIsValidCustomerLoginParameterizedTests
    {
        [Fact]
        public void IsValidCustomerLogin_Signature_IsStringString_ReturnsBool()
        {
            var mi = typeof(MySqlDbProvider).GetMethod("IsValidCustomerLogin");
            Assert.NotNull(mi);

            var parameters = mi!.GetParameters();
            Assert.Equal(2, parameters.Length);
            Assert.Equal(typeof(string), parameters[0].ParameterType);
            Assert.Equal(typeof(string), parameters[1].ParameterType);
            Assert.Equal(typeof(bool), mi.ReturnType);
        }
    }
}
