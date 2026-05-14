using System.Reflection;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_MethodExists_AcceptsStringCustomerNumber()
        {
            // Delta behavior: GetCustomerEmail now uses parameterized query (@customerNumber).
            var method = typeof(SqliteDbProvider).GetMethod("GetCustomerEmail");
            Assert.NotNull(method);
            Assert.Single(method!.GetParameters());
            Assert.Equal(typeof(string), method.GetParameters()[0].ParameterType);
        }
    }
}
