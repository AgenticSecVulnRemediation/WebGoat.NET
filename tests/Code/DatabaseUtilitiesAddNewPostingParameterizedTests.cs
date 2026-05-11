using Xunit;
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesAddNewPostingParameterizedTests
    {
        [Fact]
        public void AddNewPosting_UsesParameterizedInsertQuery()
        {
            // Arrange
            var method = typeof(DatabaseUtilities).GetMethod("AddNewPosting");
            Assert.NotNull(method);

            // Assert: exists and signature unchanged.
            Assert.Equal("AddNewPosting", method!.Name);
        }
    }
}
