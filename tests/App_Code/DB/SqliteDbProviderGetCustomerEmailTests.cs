using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_MethodExists_AfterParameterizationChange()
        {
            // Delta guard: method still exists and compiles after replacing string concatenation with parameter binding.
            var method = typeof(SqliteDbProvider).GetMethod(nameof(SqliteDbProvider.GetCustomerEmail));
            Assert.NotNull(method);
        }
    }
}
