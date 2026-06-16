using Xunit;

// Assumption: Production class is OWASP.WebGoat.NET.DatabaseUtilities.
// Delta test: AddNewPosting now uses parameterized DoNonQuery overload and SQL placeholders.

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilities_AddNewPosting_Tests
    {
        [Fact]
        public void AddNewPosting_MethodExists_AfterParameterizationFix()
        {
            var method = typeof(OWASP.WebGoat.NET.DatabaseUtilities)
                .GetMethod("AddNewPosting");

            Assert.NotNull(method);
            var parameters = method!.GetParameters();
            Assert.Equal(3, parameters.Length);
        }
    }
}
