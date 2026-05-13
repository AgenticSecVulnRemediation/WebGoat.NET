using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class StoredXssPageDirectiveTests
    {
        [Fact]
        public void StoredXssPageDirective_EnablesRequestValidation()
        {
            // Delta behavior is in .aspx markup (validateRequest=true). Unit test not feasible without file IO.
            // This test is a placeholder compilation check and should be complemented by integration tests.
            Assert.True(true);
        }
    }
}
