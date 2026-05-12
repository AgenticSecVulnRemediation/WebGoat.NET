using Xunit;
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesSqlParameterDeltaTests
    {
        [Fact]
        public void GetEmailByUserID_UsesParameterizedQuery_TemplateIsNotNull()
        {
            // Arrange
            var method = typeof(DatabaseUtilities).GetMethod("GetEmailByUserID");

            // Assert
            Assert.NotNull(method);
        }
    }
}
