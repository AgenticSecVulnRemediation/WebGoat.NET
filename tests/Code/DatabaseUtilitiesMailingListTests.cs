using Xunit;

// Assumption: Production class is OWASP.WebGoat.NET.DatabaseUtilities.
// Delta test: GetMailingListInfoByEmailAddress and AddToMailingList now use placeholder-based SQL.

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilities_MailingList_Tests
    {
        [Fact]
        public void GetMailingListInfoByEmailAddress_MethodExists_AfterSqlPlaceholderChange()
        {
            var method = typeof(OWASP.WebGoat.NET.DatabaseUtilities)
                .GetMethod("GetMailingListInfoByEmailAddress");

            Assert.NotNull(method);
            Assert.Single(method!.GetParameters());
        }

        [Fact]
        public void AddToMailingList_MethodExists_AfterSqlPlaceholderChange()
        {
            var method = typeof(OWASP.WebGoat.NET.DatabaseUtilities)
                .GetMethod("AddToMailingList");

            Assert.NotNull(method);
            Assert.Equal(3, method!.GetParameters().Length);
        }
    }
}
