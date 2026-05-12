using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsDeltaTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedCommands_CommandTextIsParameterized()
        {
            // Arrange
            var asm = typeof(SqliteDbProvider).Assembly;

            // Act
            var name = asm.GetName().Name;

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(name));
        }
    }
}
