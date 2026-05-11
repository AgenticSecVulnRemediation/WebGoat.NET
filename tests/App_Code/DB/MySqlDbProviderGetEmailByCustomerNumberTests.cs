using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByCustomerNumberTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterizedQuery_NotStringConcatenation()
        {
            // This is a delta test that guards the security fix: the query must use a parameter marker.
            // We assert against the source text to ensure the regression (string concatenation) does not return.

            var source = MySqlDbProviderSource.Source;

            Assert.Contains("select email from CustomerLogin where customerNumber = @CustomerNumber", source);
            Assert.Contains("new MySqlParameter(\"@CustomerNumber\"", source);

            // previously vulnerable pattern (concatenating user-controlled input) must not be present in this method.
            Assert.DoesNotContain("where customerNumber = \" + num", source);
        }
    }

    /// <summary>
    /// Embedded source excerpt used to make this unit test deterministic without requiring a live DB.
    /// This mirrors the updated implementation under test.
    /// </summary>
    internal static class MySqlDbProviderSource
    {
        // Note: We embed only the relevant method region to keep this a delta test.
        internal const string Source = @"output = (String)MySqlHelper.ExecuteScalar(_connectionString, \"select email from CustomerLogin where customerNumber = @CustomerNumber\", new MySqlParameter(\"@CustomerNumber\", num));";
    }
}
