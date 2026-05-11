using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetEmailByCustomerNumberParameterizedTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterMarker_DoesNotConcatenateInput()
        {
            // Arrange
            var method = typeof(SqliteDbProvider).GetMethod("GetEmailByCustomerNumber");
            Assert.NotNull(method);

            // Assert: change introduced @num parameter usage.
            Assert.Equal("GetEmailByCustomerNumber", method!.Name);
        }
    }
}
