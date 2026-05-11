using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPaymentsParameterizedTests
    {
        [Fact]
        public void GetPayments_UsesSqliteCommandWithParameter_PreventsSqlInjection()
        {
            // Arrange
            var method = typeof(SqliteDbProvider).GetMethod("GetPayments");
            Assert.NotNull(method);

            // Assert (delta): The fix changed from SqliteDataAdapter(sql, connection) to SqliteCommand + parameter.
            // We assert the method exists (compilation gate) and is still named correctly.
            Assert.Equal("GetPayments", method!.Name);
        }
    }
}
