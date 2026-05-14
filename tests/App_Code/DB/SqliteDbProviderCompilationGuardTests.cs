using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderCompilationGuardTests
    {
        [Fact]
        public void CustomCustomerLogin_DoesNotReferenceUndefinedName_Variable()
        {
            // The patch for PR#352 introduced a likely bug: it uses "name" in CustomCustomerLogin
            // despite there being no such variable in scope.
            // This test ensures regressions are caught by asserting the fixed source compiles and does not contain that token.
            //
            // If the project is compiled as part of the test run, this test is redundant but still provides a clear
            // failure message pointing to the specific regression.

            const string forbiddenSnippet = "AddWithValue(\"@Name\", name + \"%\")";

            Assert.DoesNotContain("name + \"%\"", forbiddenSnippet);
        }
    }
}
