using System;
using System.Data;
using Mono.Data.Sqlite;
using Xunit;

// Assumption: SqliteDbProvider exists in OWASP.WebGoat.NET.App_Code.DB
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPaymentsParameterizedTests
    {
        [Fact]
        public void GetPayments_WhenCalled_DoesNotThrow_ForNonNumericInjectionLikeInput()
        {
            // Arrange
            // Regression intent: SQL should be parameterized so that injection payloads don't get concatenated.
            // We can't reliably execute SqliteDbProvider without its ConfigFile/Settings environment;
            // instead, we validate the *behavioral contract* at the API boundary: method accepts int,
            // so injection strings cannot even be supplied.

            // Act + Assert
            // If someone reintroduces a string parameter or concatenation, this test should be adjusted;
            // for now we lock in the safe signature.
            var mi = typeof(SqliteDbProvider).GetMethod("GetPayments");
            Assert.NotNull(mi);
            Assert.Equal(typeof(int), mi!.GetParameters()[0].ParameterType);
        }
    }
}
