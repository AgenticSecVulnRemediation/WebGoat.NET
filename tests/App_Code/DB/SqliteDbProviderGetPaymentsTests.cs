using System;
using Xunit;

// Assumption: production code resides in namespace OWASP.WebGoat.NET.App_Code.DB
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPaymentsTests
    {
        [Fact]
        public void GetPayments_MethodExists_AndAcceptsCustomerNumberAsInt()
        {
            // Delta behavior: query inside GetPayments should now use a parameter (@customerNumber).
            // Without a DB seam, we add a deterministic regression test that will fail if the signature changes.
            var method = typeof(SqliteDbProvider).GetMethod("GetPayments", new[] { typeof(int) });
            Assert.NotNull(method);
        }
    }
}
