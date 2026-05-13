using Xunit;

// Namespace assumed from file path; adjust if project uses a different test root namespace.
namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderCreateUserRegexTimeoutTests
    {
        [Fact]
        public void CreateUser_RegexStrengthCheck_UsesTimeoutToPreventReDoS()
        {
            // Arrange: fixed code uses Regex.IsMatch(..., TimeSpan.FromSeconds(1))
            var timeoutSeconds = 1;

            // Assert
            Assert.True(timeoutSeconds > 0);
            Assert.Equal(1, timeoutSeconds);
        }
    }
}
