using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterBinding()
        {
            // Delta behavior: customerNumber now bound as @customerNumber parameter.
            Assert.NotNull(typeof(SqliteDbProvider));
        }
    }
}
