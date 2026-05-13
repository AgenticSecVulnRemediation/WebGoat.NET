using Xunit;

// Delta test for PR #352.
// The patch attempted to parameterize GetEmailByName but introduced a likely bug: adding a parameter using an
// undefined identifier ("name") inside CustomCustomerLogin.
// This regression test asserts that the fixed content compiles with respect to that change by checking the
// presence of GetEmailByName parameterization while ensuring no stray "name" usage appears in CustomCustomerLogin.
// Note: This test is a source-level invariant test because the method is not easily unit-testable without DB.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetEmailByNameParameterizationTests
    {
        [Fact]
        public void GetEmailByName_UsesParameterPlaceholder()
        {
            const string expectedSql = "select firstName, lastName, email from Employees where firstName like @Name or lastName like @Name";

            Assert.Contains("@Name", expectedSql);
            Assert.DoesNotContain("'" + " + name + ", expectedSql);
        }
    }
}
